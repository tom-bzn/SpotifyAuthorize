# SpotifyAuthorize

## Description

Library helps to authorize in Spotify API.

## Prerequisites

Familiarity with oAuth2.

## Usage

1. You may use Facade, via
```
Authorizor authorizor = new(
    clientId,
    clientSecret,
    redirectUrl);
```
2. Use authorizor.CreateLoginUrl, to produce a url to which you need to redirect the user in order to login to Spotify.
3. Create login callback page, and utilize `ExchangeCodeForTokenAsync` method. Eg. in ASP NET Core:
```
router.MapGet("/login-callback", async (HttpContext context, Authorizor api, HttpClient client) =>
{
    string? code = context.Request.Query["code"];
    if (code == null) { throw new ArgumentException("No code given."); }

    AccessTokenDetails details = await api.ExchangeCodeForTokenAsync(code);

    return details; // you may return it or not, token details will be saved inside the `Authorizor`
});
```
4. You may use later RefreshTokenAsync method if needed.
