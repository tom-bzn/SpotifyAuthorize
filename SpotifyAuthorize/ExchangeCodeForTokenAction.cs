using System.Data;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace SpotifyAuthorize;

internal class ExchangeCodeForTokenAction(string clientId, string clientSecret, string redirectUrl)
{
    private readonly string _clientId = clientId;

    private readonly string _clientSecret = clientSecret;

    private readonly string _redirectUrl = redirectUrl;

    public async Task<AccessTokenDetails> Perform(string code, HttpClient client)
    {
        HttpRequestMessage request = new(HttpMethod.Post, "https://accounts.spotify.com/api/token");
        request.Headers.Add("Authorization", PrepareAuthHeaderValue());
        request.Content = new FormUrlEncodedContent(PrepareContentData(code));

        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<AccessTokenDetails>(new JsonSerializerOptions(JsonSerializerDefaults.Web)
        {
            PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
        }) ?? throw new NoNullAllowedException();
    }

    private string PrepareAuthHeaderValue()
    {
        var plainTextBytes = Encoding.UTF8.GetBytes($"{_clientId}:{_clientSecret}");
        var encodedCredentials = Convert.ToBase64String(plainTextBytes);

        return $"Basic {encodedCredentials}";
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
