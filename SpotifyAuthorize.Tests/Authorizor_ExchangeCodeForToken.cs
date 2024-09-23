using System.Net;
using System.Net.Http.Json;
using Moq;
using Moq.Protected;
using SpotifyAuthorize.Models;

namespace SpotifyAuthorize.Tests;

public class Authorizor_ExchangeCodeForToken
{
    [Fact]
    public async Task Xyz()
    {
        var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);

        // access_token     string  An access token that can be provided in subsequent calls, for example to Spotify Web API services.
        // token_type       string  How the access token may be used: always "Bearer".
        // scope            string  A space-separated list of scopes which have been granted for this access_token
        // expires_in       int     The time period (in seconds) for which the access token is valid.
        // refresh_token    string  See refreshing tokens.

        var dto = new
        {
            access_token = "fake_access_token",
            token_type = "Bearer",
            scope = "abra dab",
            expires_in = 3600,
            refresh_token = "fake_refresh_token"
        };
        var mockResponse = new HttpResponseMessage
        {
            StatusCode = HttpStatusCode.OK,
            Content = JsonContent.Create(dto)
        };

        // mock.Setup(foo => foo.DoSomethingAsync().Result).Returns(true);

        // mockHandler.Protected()
        //     .Setup<int>("Execute")
        //     .Returns(5);

        // mock.Protected()
        //     .Setup<Task>("DoSomethingInternal", ItExpr.IsAny<MyContext>())
        //     .ThrowsAsync(new TaskCanceledException());

        //mockHandler.Setup(handler => handler.SendAsync().Result).Returns(true);

        // mockHandler
        //     .Protected()
        //     .Setup<Task<HttpResponseMessage>>(
        //         "SendAsync",
        //         ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Get),
        //         ItExpr.IsAny<CancellationToken>())
        //     .ReturnsAsync(mockResponse);


        mockHandler
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.Is<HttpRequestMessage>(m => m.Method == HttpMethod.Post),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(mockResponse);

        Authorizor sut = new(new HttpClient(mockHandler.Object), "client_id", "client_secret", "https://example.com");

        AccessTokenDetails tokenDetails = await sut.ExchangeCodeForTokenAsync("CODETOEXCHANGE");

        Assert.Equal("fake_access_token", tokenDetails.AccessToken);
    }
}
