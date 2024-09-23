using System.Web;

namespace SpotifyAuthorize;

internal class CreateLoginUrlAction
{
    public static string Perform(string clientId, IEnumerable<string> scope, string redirectUrl)
    {
        if (false == scope.Any())
        {
            throw new ArgumentException("You should provide at least one scope.");
        }

        if (!Uri.IsWellFormedUriString(redirectUrl, UriKind.Absolute))
        {
            throw new UriFormatException("Invalid redirectUrl provided.");
        }

        // cannot instantiate otherwise eg. with new()
        var qs = HttpUtility.ParseQueryString(string.Empty);

        qs.Add("response_type", "code");
        qs.Add("client_id", clientId);
        qs.Add("scope", string.Join(' ', scope));
        qs.Add("redirect_uri", redirectUrl);
        qs.Add("state", Guid.NewGuid().ToString());

        UriBuilder uriBuilder = new("https://accounts.spotify.com/authorize/") { Query = qs.ToString() };

        return uriBuilder.Uri.ToString();
    }
}
