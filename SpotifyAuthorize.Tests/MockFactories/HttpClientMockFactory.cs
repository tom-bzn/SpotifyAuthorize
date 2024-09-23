using Moq;

namespace SpotifyAuthorize.Tests.MockFactories;

public class HttpClientMockFactory
{
    public static HttpClient CreateMockWithNoConfig()
    {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        return new HttpClient(mockHandler.Object);
    }
}
