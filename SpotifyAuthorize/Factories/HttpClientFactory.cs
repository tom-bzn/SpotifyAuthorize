using System.Net.Http.Headers;
using System.Text;

namespace SpotifyAuthorize.Factories;

public static class HttpClientFactory
{
    public static HttpClient Create(string clientId, string clientSecret)
    {
        var handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        var client = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://accounts.spotify.com/api/token/")
        };

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", PrepareAuthHeaderValue(clientId, clientSecret));

        return client;
    }

    private static string PrepareAuthHeaderValue(string clientId, string clientSecret)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes($"{clientId}:{clientSecret}");
        var encodedCredentials = Convert.ToBase64String(plainTextBytes);

        return encodedCredentials;
    }
}
