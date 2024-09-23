using SpotifyAuthorize.Tests.MockFactories;

namespace SpotifyAuthorize.Tests;

public class Authorizor_CreateLoginUrl_ThrowsExceptionOnInvalidInput
{
    private readonly HttpClient _httpClientMock = HttpClientMockFactory.CreateMockWithNoConfig();

    [Fact]
    public void ShouldThrowArgumentExceptionOnEmptyScope()
    {
        Assert.Throws<ArgumentException>(() => new Authorizor(_httpClientMock, "client_id", "client_secret", "https://example.com")
            .CreateLoginUrl([]));
    }

    [Fact]
    public void ShouldThrowExceptionOnInvalidRedirectUrl()
    {
        Assert.Throws<UriFormatException>(() => new Authorizor(_httpClientMock, "client_id", "client_secret", "invalid url")
            .CreateLoginUrl(["scope1"]));
    }
}
