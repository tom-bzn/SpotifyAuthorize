namespace SpotifyAuthorize.Tests;

public class Authorizor_CreateLoginUrl_ThrowsExceptionOnInvalidInput
{
    private readonly Authorizor Sut = new("client_id", "client_secret", "https://example.com");

    [Fact]
    public void ShouldThrowArgumentExceptionOnEmptyScope()
    {
        Assert.Throws<ArgumentException>(() => Sut.CreateLoginUrl([]));
    }

    [Fact]
    public void ShouldThrowExceptionOnInvalidRedirectUrl()
    {
        Assert.Throws<UriFormatException>(() => new Authorizor("client_id", "client_secret", "invalid url").CreateLoginUrl(["scope1"]));
    }
}
