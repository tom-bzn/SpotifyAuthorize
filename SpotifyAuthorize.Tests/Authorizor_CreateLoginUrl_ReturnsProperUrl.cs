using System.Web;

namespace SpotifyAuthorize.Tests;

public class Authorizor_CreateLoginUrl_ReturnsProperUrl
{
    [Fact]
    public void ShouldReturnProperUrlOnSimpleInput()
    {
        string clientId = "client_id";
        string clientSecret = "client_secret";
        string redirectUrl = "https://example.com";
        Authorizor sut = new(clientId, clientSecret, redirectUrl);
        string[] scope = ["scope1"];

        string url = sut.CreateLoginUrl(scope);

        AssertUrl(url, clientId, redirectUrl, scope);
    }

    [Fact]
    public void ShouldReturnProperUrlOnMoreComplicatedInput()
    {
        string clientId = Guid.NewGuid().ToString();
        string clientSecret = Guid.NewGuid().ToString();
        string redirectUrl = "https://example.com?key=value&key2=value2";
        Authorizor sut = new(clientId, clientSecret, redirectUrl);
        string[] scope = ["scope1", "scope2", "scope_!23"];

        string url = sut.CreateLoginUrl(scope);

        AssertUrl(url, clientId, redirectUrl, scope);
    }

    private static void AssertUrl(string url, string clientId, string redirectUrl, IEnumerable<string> scope)
    {
        Assert.True(Uri.IsWellFormedUriString(url, UriKind.Absolute));

        var parsedUrl = new Uri(url);
        var parsedQueryString = HttpUtility.ParseQueryString(parsedUrl.Query);

        var pairs = new Dictionary<string, string>
        {
            {"response_type", "code"},
            {"client_id", clientId},
            {"scope", string.Join(' ', scope)},
            {"redirect_uri", redirectUrl}
        };

        foreach (KeyValuePair<string, string> pair in pairs)
        {
            Assert.Equal(pair.Value, parsedQueryString.Get(pair.Key));
        }
    }
}
