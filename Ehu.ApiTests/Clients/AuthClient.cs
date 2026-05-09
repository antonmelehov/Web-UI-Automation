using System.Net.Http.Headers;
using System.Text.Json;
using Ehu.ApiTests.Configuration;
using Ehu.ApiTests.Models;

namespace Ehu.ApiTests.Clients;

public class AuthClient : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly bool _disposeHttpClient;

    public AuthClient(HttpClient? httpClient = null)
    {
        if (httpClient is null)
        {
            _httpClient = new HttpClient();
            _disposeHttpClient = true;
        }
        else
        {
            _httpClient = httpClient;
            _disposeHttpClient = false;
        }
    }

    public async Task<TokenResponse> GetTokenAsync(CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, TestConfiguration.TokenUrl);

        request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        request.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["client_id"] = TestConfiguration.ClientId,
            ["client_secret"] = TestConfiguration.ClientSecret,
            ["scope"] = TestConfiguration.Scope,
            ["grant_type"] = TestConfiguration.GrantType
        });

        using var response = await _httpClient.SendAsync(request, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            throw new HttpRequestException(
                $"Token request failed. Status code: {(int)response.StatusCode}. Response: {responseContent}");
        }

        var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(
            responseContent,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

        if (tokenResponse is null)
        {
            throw new InvalidOperationException("Token response could not be deserialized.");
        }

        if (string.IsNullOrWhiteSpace(tokenResponse.AccessToken))
        {
            throw new InvalidOperationException("Access token is missing in the token response.");
        }

        return tokenResponse;
    }

    public async Task<string> GetAccessTokenAsync(CancellationToken cancellationToken = default)
    {
        var tokenResponse = await GetTokenAsync(cancellationToken);
        return tokenResponse.AccessToken;
    }

    public void Dispose()
    {
        if (_disposeHttpClient)
        {
            _httpClient.Dispose();
        }
    }
}