using System.Data;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using SpotifyAuthorize.Models;

namespace SpotifyAuthorize.Actions;

internal class ExchangeCodeForTokenAction(string redirectUrl)
{
    private readonly string _redirectUrl = redirectUrl;

    public async Task<AccessTokenDetails> Perform(string code, HttpClient client)
    {
        HttpRequestMessage request = new(HttpMethod.Post, "")
        {
            Content = new FormUrlEncodedContent(PrepareContentData(code))
        };

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AccessTokenDetails>(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }) ?? throw new NoNullAllowedException();
    }

    private Dictionary<string, string> PrepareContentData(string code)
    {
        return new Dictionary<string, string>
        {
            {"code", code},
            {"redirect_uri", _redirectUrl},
            {"grant_type", "authorization_code"}
        };
    }
}
