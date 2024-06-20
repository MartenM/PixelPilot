using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PixelPilot.Common;
using PixelPilot.Common.Logging;
using PixelPilot.PixelHttpClient.Responses;
using PixelPilot.PixelHttpClient.Responses.Auth;
using PixelPilot.PixelHttpClient.Responses.Collections;
using PixelPilot.PixelHttpClient.Responses.visible;

namespace PixelPilot.PixelHttpClient;

/// <summary>
/// Used to make HTTP API request instead of using the websocket.
/// Required for obtaining information used to join a world.
/// </summary>
public class PixelApiClient : IDisposable
{
    private readonly ILogger _logger = LogManager.GetLogger("API");
    private readonly HttpClient _client;

    private readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private string _version = "0.0.0";
    
    /// <summary>
    /// PixelApiClient that authenticates using a token.
    /// Does not verify validity.
    /// </summary>
    /// <param name="accountToken">A valid account token</param>
    public PixelApiClient(string accountToken)
    {
        SetVersion();
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", $"PixelPilot/{_version}");
        _client.DefaultRequestHeaders.Add("Authorization", accountToken);
    }

    /// <summary>
    /// PixelApiClient that authenticates using email and password.
    /// </summary>
    /// <param name="email">User e-mail</param>
    /// <param name="password">User password</param>
    /// <exception cref="PixelApiException">When the login failed</exception>
    /// <exception cref="InvalidOperationException">Something went very wrong</exception>
    public PixelApiClient(string email, string password)
    {
        SetVersion();
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", $"PixelPilot/{_version}");
        
        // Retrieve token
        var response = GetAuth(email, password);
        response.Wait();

        var result = response.Result;

        // Auth failure, we cannot continue.
        if (result is AuthErrorResponse authResponse)
        {
            throw new PixelApiException("Failed to retrieve login token using the given information.");
        }
        
        // Success
        if (result is AuthSuccessResponse successResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", successResponse.Token);
            return;
        }

        throw new InvalidOperationException("Something went wrong while converting your email and password.");
    }

    private void SetVersion()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fileVersionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
        _version = fileVersionInfo.ProductVersion?.Split("+")[0] ?? "0.0.0";
    }
    
    /// <summary>
    /// Request a join key for the given room from the API server.
    /// </summary>
    /// <param name="roomType">The room type</param>
    /// <param name="roomId">ID of the room</param>
    /// <returns></returns>
    public async Task<JoinKeyResponse?> GetJoinKey(string roomType, string roomId)
    {
        var roomTokenUrl = $"{EndPoints.ApiEndpoint}/api/joinkey/{roomType}/{roomId}";
        _logger.LogInformation($"API Request: {roomTokenUrl}");

        try
        {
            return await JsonSerializer.DeserializeAsync<JoinKeyResponse>(await _client.GetStreamAsync(roomTokenUrl));
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Forbidden)
                throw new PixelApiException("Couldn't fetch the join key. Are you sure your token is still valid?");

            throw;
        }
    }

    private static List<string> _roomTypesCache = new();
    /// <summary>
    /// Request the available room types from the game server.
    /// </summary>
    /// <returns>A list of room types</returns>
    public async Task<List<string>?> GetRoomTypes()
    {
        if (_roomTypesCache.Count != 0) return new List<string>(_roomTypesCache);
        
        var apiUrl = $"{EndPoints.GameHttpEndpoint}/listroomtypes";
        _logger.LogInformation($"API Request: {apiUrl}");

        _roomTypesCache = (await JsonSerializer.DeserializeAsync<List<string>>(await _client.GetStreamAsync(apiUrl)))!;
        return _roomTypesCache;
    }
    
    /// <summary>
    /// Retrieves the mappings from the game API.
    /// </summary>
    /// <returns>A <see cref="MappingsResponse"/> containing the mappings, or null if the mappings are not available.</returns>
    public async Task<MappingsResponse?> GetMappings()
    {
        var apiUrl = $"{EndPoints.GameHttpEndpoint}/mappings";
        _logger.LogInformation($"API Request: {apiUrl}");

        var mapping = await JsonSerializer.DeserializeAsync<Dictionary<string, int>>(await _client.GetStreamAsync(apiUrl));
        return mapping == null ? null : new MappingsResponse(mapping);
    }

    public async Task<IAuthResponse> GetAuth(string email, string password)
    {
        var apiUrl = $"{EndPoints.ApiEndpoint}/api/collections/users/auth-with-password";
        _logger.LogInformation($"API Request: {apiUrl}");
        
        var data = new
        {
            identity = email,
            password
        };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(apiUrl, content);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<AuthErrorResponse>(await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        return JsonSerializer.Deserialize<AuthSuccessResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()) ?? throw new InvalidOperationException();
    }

    /// <summary>
    /// Fetches the worlds of the current authenticated player.
    /// </summary>
    /// <param name="page">Page to be fetched</param>
    /// <param name="perPage">Entries per page</param>
    /// <param name="qb">Query builder</param>
    /// <returns>The page requested</returns>
    /// <exception cref="PixelApiException">When the rooms worlds could not be fetched</exception>
    public async Task<CollectionResponse<WorldEntry>> GetOwnedWorlds(int page, int perPage, QueryArgumentBuilder? qb = null)
    {
        if (page == 0)
            throw new PixelApiException("Pages start at 1. Not 0!");
        
        var apiUrl = $"{EndPoints.ApiEndpoint}/api/collections/worlds/records?page={page}&perPage={perPage}{qb?.Build() ?? ""}";
        _logger.LogInformation($"API Request: {apiUrl}");

        var worldCollection =
            await JsonSerializer.DeserializeAsync<CollectionResponse<WorldEntry>>(await _client.GetStreamAsync(apiUrl), _jsonOptions);
        return worldCollection ?? throw new PixelApiException("An unknown exception occured while attempting to fetch the worlds");
    }

    /// <summary>
    /// Asynchronously retrieves a paginated collection of player entries with optional filters.
    /// </summary>
    /// <param name="page">The page number to retrieve. Must be greater than 0.</param>
    /// <param name="perPage">The number of player entries to retrieve per page.</param>
    /// <param name="qb">Query builder</param>
    /// <returns>A task representing the asynchronous operation, with a result of <see cref="CollectionResponse{PlayerEntry}"/> containing the player entries.</returns>
    /// <exception cref="PixelApiException">Thrown when the page number is less than 1 or when an unknown error occurs during the fetch operation.</exception>
    public async Task<CollectionResponse<PlayerEntry>> GetPlayers(int page, int perPage, QueryArgumentBuilder? qb = null)
    {
        if (page == 0)
            throw new PixelApiException("Pages start at 1. Not 0!");
        
        var apiUrl = $"{EndPoints.ApiEndpoint}/api/collections/public_profiles/records?page={page}&perPage={perPage}{qb?.Build() ?? ""}";
        _logger.LogInformation($"API Request: {apiUrl}");

        var worldCollection =
            await JsonSerializer.DeserializeAsync<CollectionResponse<PlayerEntry>>(await _client.GetStreamAsync(apiUrl), _jsonOptions);
        return worldCollection ?? throw new PixelApiException("An unknown exception occured while attempting to fetch the worlds");
    }
    
    public async Task<CollectionResponse<WorldEntry>> GetPublicWorlds(int page, int perPage, QueryArgumentBuilder? qb = null)
    {
        if (page == 0)
            throw new PixelApiException("Pages start at 1. Not 0!");
        
        var apiUrl = $"{EndPoints.ApiEndpoint}/api/collections/public_worlds/records?page={page}&perPage={perPage}{qb?.Build() ?? ""}";
        _logger.LogInformation($"API Request: {apiUrl}");

        var worldCollection =
            await JsonSerializer.DeserializeAsync<CollectionResponse<WorldEntry>>(await _client.GetStreamAsync(apiUrl), _jsonOptions);
        return worldCollection ?? throw new PixelApiException("An unknown exception occured while attempting to fetch the worlds");
    }

    public async Task<WorldEntry?> GetPublicWorld(string id)
    {
        var query = new QueryArgumentBuilder()
            .AddFilter("id", id);

        var worlds = await GetPublicWorlds(1, 1, query);
        return worlds.TotalItems == 0 ? null : worlds.Items.First();
    }
    
    /// <summary>
    /// Asynchronously retrieves a player entry by username.
    /// </summary>
    /// <param name="username">The username of the player to retrieve.</param>
    /// <returns>
    /// A task representing the asynchronous operation, with a result of <see cref="PlayerEntry"/> containing the player entry,
    /// or <c>null</c> if no player with the specified username is found.
    /// </returns>
    public async Task<PlayerEntry?> GetPlayer(string username)
    {
        var players = await GetPlayers(1, 1, new QueryArgumentBuilder().AddFilter("username", username));
        if (players.TotalItems == 0) return null;
        return players.Items[0];
    }
    
    /// <summary>
    /// Fetch the visible worlds in the browser found in the lobby.
    /// </summary>
    /// <returns>The visible rooms</returns>
    /// <exception cref="PixelApiException">When the worlds could not be fetched.</exception>
    public async Task<PixelWalkerWorldsResponse> GetVisibleWorlds()
    {
        var apiUrl = $"{EndPoints.GameHttpEndpoint}/room/list/{_roomTypesCache.First()}";
        _logger.LogInformation($"API Request: {apiUrl}");
        
        var worldCollection =
            await JsonSerializer.DeserializeAsync<PixelWalkerWorldsResponse>(await _client.GetStreamAsync(apiUrl), _jsonOptions);
        return worldCollection ?? throw new PixelApiException("An unknown exception occured while attempting to fetch the worlds");
    }

    /// <summary>
    /// Get the raw bytes of the minimap.
    /// The format is PNG
    /// </summary>
    /// <param name="world"></param>
    /// <returns>PNG Byte[]</returns>
    public async Task<byte[]> GetMinimap(WorldEntry world)
    {
        var collectionId = "rhrbt6wqhc4s0cp";
        var apiUrl = $"{EndPoints.ApiEndpoint}/api/files/{collectionId}/{world.Id}/{world.Minimap}";

        byte[] bytes = await _client.GetByteArrayAsync(apiUrl);
        return bytes;
    }

    /// <summary>
    /// Get the raw bytes of the minimap.
    /// The format is PNG.
    /// </summary>
    /// <param name="worldId">World ID</param>
    /// <returns>Byte[] or null if world could not be found.</returns>
    public async Task<byte[]?> GetMinimap(string worldId)
    {
        var world = await GetPublicWorld(worldId);
        return world == null ? null : await GetMinimap(world);
    }
    
    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}