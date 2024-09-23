namespace SpotifyAuthorize.Models;

/**
 * @see https://developer.spotify.com/documentation/web-api/tutorials/code-flow
 */
public record class AccessTokenDetails
(
    string AccessToken,
    string TokenType,
    string Scope,
    int ExpiresIn,
    string RefreshToken
);
