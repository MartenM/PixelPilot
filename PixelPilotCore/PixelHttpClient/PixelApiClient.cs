using System.Text.Json;
using Microsoft.Extensions.Logging;
using PixelPilot.Models;
using PixelPilot.PixelHttpClient.Responses;

namespace PixelPilot.PixelHttpClient;

/**
 * Used to interact with the HTTP endpoint.
 */
public class PixelApiClient : IDisposable
{
    private readonly ILogger _logger = LogManager.GetLogger("API");
    private readonly HttpClient _client;

    public PixelApiClient(string accountToken)
    {
        _client = new HttpClient();
        _client.DefaultRequestHeaders.Add("User-Agent", "PixelPilot/1.0.0");
        _client.DefaultRequestHeaders.Add("Authorization", accountToken);
    }

    /**
     * Request the join key from the API server.
     * Either returns the join key or NULL.
     */
    public async Task<JoinKeyResponse?> GetJoinKey(RoomType roomType, string roomId)
    {
        var roomTokenUrl = $"{EndPoints.ApiEndpoint}/api/joinkey/{roomType.AsString()}/{roomId}";
        _logger.LogInformation($"API Request: {roomTokenUrl}");
        return await JsonSerializer.DeserializeAsync<JoinKeyResponse>(await _client.GetStreamAsync(roomTokenUrl));
    }
    
    /**
     * Request the join key from the API server.
     * Either returns the join key or NULL.
     */
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
    
    public void Dispose()
    {
        _client.Dispose();
        GC.SuppressFinalize(this);
    }
}