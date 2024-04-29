using System.Net;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using PixelPilot.Models;
using PixelPilot.PixelHttpClient.Responses;

namespace PixelPilot.PixelHttpClient;

/// <summary>
/// Used to make HTTP API request instead of using the websocket.
/// Required for obtaining information used to join a world.
/// </summary>
public class PixelApiClient : IDisposable
{
    private readonly ILogger _logger = LogManager.GetLogger("API");
    private readonly HttpClient _client;

    /// <summary>
    /// PixelApiClient that authenticates using a token.
    /// Does not verify validity.
    /// </summary>
    /// <param name="accountToken">A valid account token</param>
    public PixelApiClient(string accountToken)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "PixelPilot/1.0.0");
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
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "PixelPilot/1.0.0");
        
        // Retrieve token
        var response = GetAuth(email, password);
        response.Wait();

        // Auth failure, we cannot continue.
        if (response.Result is AuthErrorResponse)
            throw new PixelApiException("Failed to retrieve login token using the given information.");
        
        // Success
        if (response.Result is AuthSuccessResponse successResponse)
        {
            _client.DefaultRequestHeaders.Add("Authorization", successResponse.Token);
            return;
        }

        throw new InvalidOperationException("Something went wrong while converting your email and password.");
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
        return await JsonSerializer.DeserializeAsync<JoinKeyResponse>(await _client.GetStreamAsync(roomTokenUrl));
    }
    
    /// <summary>
    /// Request the available room types from the game server.
    /// </summary>
    /// <returns>A list of room types</returns>
    public async Task<List<string>?> GetRoomTypes()
    {
        var apiUrl = $"{EndPoints.GameHttpEndpoint}/listroomtypes";
        _logger.LogInformation($"API Request: {apiUrl}");
        return await JsonSerializer.DeserializeAsync<List<string>>(await _client.GetStreamAsync(apiUrl));
    }
    
    /**
     * Request the join key from the API server.
     * Either returns the join key or NULL.
     */
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
            password = password
        };
        var content = new StringContent(JsonSerializer.Serialize(data), Encoding.UTF8, "application/json");
        var response = await _client.PostAsync(apiUrl, content);

        if (response.StatusCode != HttpStatusCode.OK)
        {
            return JsonSerializer.Deserialize<AuthErrorResponse>(await response.Content.ReadAsStringAsync()) ?? throw new InvalidOperationException();
        }
        return JsonSerializer.Deserialize<AuthSuccessResponse>(await response.Content.ReadAsStringAsync(), new JsonSerializerOptions()) ?? throw new InvalidOperationException();
    }
    
    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}