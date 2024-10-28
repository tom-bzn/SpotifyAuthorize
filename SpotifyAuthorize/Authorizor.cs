using SpotifyAuthorize.Actions;
using SpotifyAuthorize.Factories;
using SpotifyAuthorize.Models;

namespace SpotifyAuthorize;

/// <summary>
/// Spotify Authorize facade
/// </summary>
/// <param name="redirectUrl">The one, user should be redirected to after successful logging to Spotify</param>
public class Authorizor(string clientId, string clientSecret, string redirectUrl, HttpClient? client = null)
{
    private readonly HttpClient _httpClient = client ?? HttpClientFactory.Create(clientId, clientSecret);

    private readonly string _clientId = clientId;

    private readonly string _redirectUrl = redirectUrl;

    private AccessTokenDetails? _obtainedTokenDetails;

    private DateTime _obtainedTokenExpiryTime;

    public string AccessToken => _obtainedTokenDetails?.AccessToken ?? throw new InvalidOperationException("No token available.");

    /// <summary>
    /// Creates a url to which you should redirect a user to log in.
    /// </summary>
    public string CreateLoginUrl(IEnumerable<string> scope)
    {
        return CreateLoginUrlAction.Perform(_clientId, scope, _redirectUrl);
    }

    public async Task<AccessTokenDetails> ExchangeCodeForTokenAsync(string code)
    {
        _obtainedTokenDetails = await new ExchangeCodeForTokenAction(_redirectUrl).Perform(code, _httpClient);
        _obtainedTokenExpiryTime = DateTime.UtcNow.AddSeconds(_obtainedTokenDetails.ExpiresIn);

        return _obtainedTokenDetails;
    }

    public async Task<AccessTokenDetails> RefreshTokenAsync()
    {
        if (_obtainedTokenDetails == null)
        {
            throw new InvalidOperationException("There is no access token details stored, thus we don't have a refresh token.");
        }

        _obtainedTokenDetails = await new RefreshTokenAction(_clientId).Perform(_obtainedTokenDetails.RefreshToken, _httpClient);
        _obtainedTokenExpiryTime = DateTime.UtcNow.AddSeconds(_obtainedTokenDetails.ExpiresIn);

        return _obtainedTokenDetails;
    }

    public bool IsTokenExpired()
    {
        return DateTime.UtcNow >= _obtainedTokenExpiryTime;
    }
}
