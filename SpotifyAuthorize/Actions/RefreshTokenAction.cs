using System.Data;
using System.Net.Http.Json;
using System.Text.Json;
using SpotifyAuthorize.Models;

namespace SpotifyAuthorize.Actions;

internal class RefreshTokenAction(string clientId)
{
    private readonly string _clientId = clientId;

    public async Task<AccessTokenDetails> Perform(string refreshToken, HttpClient client)
    {
        HttpRequestMessage request = new(HttpMethod.Post, "https://accounts.spotify.com/api/token")
        {
            Content = new FormUrlEncodedContent(PrepareContentData(refreshToken))
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AccessTokenDetails>(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }) ?? throw new NoNullAllowedException();
    }

    private Dictionary<string, string> PrepareContentData(string refreshToken)
    {
        return new Dictionary<string, string>
        {
            {"client_id", _clientId},
            {"refresh_token", refreshToken},
            {"grant_type", "refresh_token"}
        };
    }
}
