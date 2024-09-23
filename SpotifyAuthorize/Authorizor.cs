using SpotifyAuthorize.Actions;
using SpotifyAuthorize.Models;

namespace SpotifyAuthorize;

/// <summary>
/// Spotify Authorize facade
/// </summary>
/// <param name="redirectUrl">The one, user should be redirected to after successful logging to Spotify</param>
public class Authorizor(HttpClient client, string clientId, string clientSecret, string redirectUrl)
{
    private readonly HttpClient _httpClient = client;

    private readonly string _clientId = clientId;

    private readonly string _clientSecret = clientSecret;

    private readonly string _redirectUrl = redirectUrl;

    private AccessTokenDetails? _accessTokenDetails = null;

    public string? AccessToken { get => _accessTokenDetails?.AccessToken; }

    /// <summary>
    /// Creates a url to which you should redirect a user to log in.
    /// </summary>
    public string CreateLoginUrl(IEnumerable<string> scope)
    {
        return CreateLoginUrlAction.Perform(_clientId, scope, _redirectUrl);
    }

    public async Task<AccessTokenDetails> ExchangeCodeForTokenAsync(string code)
    {
        _accessTokenDetails = await new ExchangeCodeForTokenAction(_clientId, _clientSecret, _redirectUrl).Perform(code, _httpClient);
        return _accessTokenDetails;
    }

    public async Task<AccessTokenDetails> RefreshTokenAsync()
    {
        if (_accessTokenDetails == null)
        {
            throw new InvalidOperationException("There is no access token details stored, thus we don't have a refresh token.");
        }

        _accessTokenDetails = await new RefreshTokenAction(_clientId).Perform(_accessTokenDetails.RefreshToken, _httpClient);
        return _accessTokenDetails;
    }
}
