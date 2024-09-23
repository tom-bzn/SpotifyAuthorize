namespace SpotifyAuthorize;

public static class HttpClientFactory
{
    public static HttpClient Create()
    {
        var handler = new SocketsHttpHandler
        {
            PooledConnectionLifetime = TimeSpan.FromMinutes(2)
        };

        return new HttpClient(handler)
        {
            BaseAddress = new Uri("https://accounts.spotify.com/api/token/")
        };
    }
}
