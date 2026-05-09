using System.Net.Http.Json;
using System.Text.Json;
using Ehu.ApiTests.Configuration;
using Ehu.ApiTests.Models;
using Serilog;

namespace Ehu.ApiTests.Clients;

public class BooksApiClient
{
    private readonly HttpClient _httpClient;

    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public BooksApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HttpResponseMessage> CreateBookAsync(BookCreateRequest request, CancellationToken cancellationToken = default)
    {
        Log.Debug("Sending POST request to create a book");

        return await _httpClient.PostAsJsonAsync(TestConfiguration.BooksEndpoint, request, cancellationToken);
    }

    public async Task<HttpResponseMessage> GetAllBooksAsync(CancellationToken cancellationToken = default)
    {
        Log.Debug("Sending GET request for all books");

        return await _httpClient.GetAsync(TestConfiguration.BooksEndpoint, cancellationToken);
    }

    public async Task<HttpResponseMessage> GetBookByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await GetBookByIdAsync(id.ToString(), cancellationToken);
    }

    public async Task<HttpResponseMessage> GetBookByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        Log.Debug("Sending GET request for book by id: {BookId}", id);

        return await _httpClient.GetAsync($"{TestConfiguration.BooksEndpoint}/{id}", cancellationToken);
    }

    public async Task<HttpResponseMessage> UpdateBookAsync(Guid id, BookUpdateRequest request, CancellationToken cancellationToken = default)
    {
        return await UpdateBookAsync(id.ToString(), request, cancellationToken);
    }

    public async Task<HttpResponseMessage> UpdateBookAsync(string id, BookUpdateRequest request, CancellationToken cancellationToken = default)
    {
        Log.Debug("Sending PUT request to update book with id: {BookId}", id);

        return await _httpClient.PutAsJsonAsync($"{TestConfiguration.BooksEndpoint}/{id}", request, cancellationToken);
    }

    public async Task<HttpResponseMessage> DeleteBookAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DeleteBookAsync(id.ToString(), cancellationToken);
    }

    public async Task<HttpResponseMessage> DeleteBookAsync(string id, CancellationToken cancellationToken = default)
    {
        Log.Debug("Sending DELETE request for book with id: {BookId}", id);

        return await _httpClient.DeleteAsync($"{TestConfiguration.BooksEndpoint}/{id}", cancellationToken);
    }

    public async Task<T?> ReadResponseAsync<T>(HttpResponseMessage response, CancellationToken cancellationToken = default)
    {
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (string.IsNullOrWhiteSpace(responseContent))
        {
            return default;
        }

        return JsonSerializer.Deserialize<T>(responseContent, JsonOptions);
    }
}