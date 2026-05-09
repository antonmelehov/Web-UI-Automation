using System.Net;
using Ehu.ApiTests.Clients;
using Ehu.ApiTests.Models;
using Serilog;
using Shouldly;

namespace Ehu.ApiTests.Tests;

[TestFixture]
[Category("API")]
[Category("Books")]
[Category("Create")]
public class CreateBookTests : BaseApiTest
{
    [Test]
    public async Task Create_Book_With_Valid_Data_Should_Return_201()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var request = CreateValidBookRequest();

        var response = await booksApiClient.CreateBookAsync(request);
        var responseBody = await response.Content.ReadAsStringAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var createdBook = await booksApiClient.ReadResponseAsync<BookResponse>(response);

        createdBook.ShouldNotBeNull();
        createdBook.Id.ShouldNotBe(Guid.Empty);

        RegisterCreatedBook(createdBook.Id);
    }

    [Test]
    public async Task Create_Book_With_Valid_Data_Should_Return_Response_Body_Matching_Request()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var request = CreateValidBookRequest();

        var response = await booksApiClient.CreateBookAsync(request);

        response.StatusCode.ShouldBe(HttpStatusCode.Created);

        var createdBook = await booksApiClient.ReadResponseAsync<BookResponse>(response);

        createdBook.ShouldNotBeNull();
        createdBook.Id.ShouldNotBe(Guid.Empty);
        createdBook.Title.ShouldBe(request.Title);
        createdBook.Author.ShouldBe(request.Author);
        createdBook.PublishedDate.ShouldBe(request.PublishedDate);

        if (!string.IsNullOrWhiteSpace(request.Isbn))
        {
            createdBook.Isbn.ShouldBe(request.Isbn);
        }


        RegisterCreatedBook(createdBook.Id);
    }

    [Test]
    public async Task Create_Duplicate_Book_Should_Not_Return_201()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var request = CreateValidBookRequest();

        var firstResponse = await booksApiClient.CreateBookAsync(request);
        firstResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        var firstCreatedBook = await booksApiClient.ReadResponseAsync<BookResponse>(firstResponse);
        firstCreatedBook.ShouldNotBeNull();
        firstCreatedBook.Id.ShouldNotBe(Guid.Empty);
        RegisterCreatedBook(firstCreatedBook.Id);

        var duplicateResponse = await booksApiClient.CreateBookAsync(request);

        duplicateResponse.StatusCode.ShouldNotBe(HttpStatusCode.Created);
        duplicateResponse.StatusCode.ShouldBeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.Conflict);
    }

    private static BookCreateRequest CreateValidBookRequest()
    {
        var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];

        return new BookCreateRequest
        {
            Title = $"War and Peace {uniqueSuffix}",
            Author = "Leo Tolstoy",
            PublishedDate = DateTime.UtcNow,
            Isbn = $"978-0199232-{uniqueSuffix}",
            IsAvailable = true
        };
    }
}