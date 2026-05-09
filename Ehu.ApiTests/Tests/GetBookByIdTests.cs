using System.Net;
using Ehu.ApiTests.Clients;
using Ehu.ApiTests.Models;
using Shouldly;

namespace Ehu.ApiTests.Tests;

[TestFixture]
[Category("API")]
[Category("Books")]
[Category("GetById")]
public class GetBookByIdTests : BaseApiTest
{
    [Test]
    public async Task Get_Book_By_Valid_Id_Should_Return_200_And_Correct_Data()
    {
        var booksApiClient = new BooksApiClient(HttpClient);

        var createdBook = await CreateBookForTestAsync(booksApiClient);

        var response = await booksApiClient.GetBookByIdAsync(createdBook.Id);

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var returnedBook = await booksApiClient.ReadResponseAsync<BookResponse>(response);

        returnedBook.ShouldNotBeNull();
        returnedBook.Id.ShouldBe(createdBook.Id);
        returnedBook.Title.ShouldBe(createdBook.Title);
        returnedBook.Author.ShouldBe(createdBook.Author);
        returnedBook.PublishedDate.ShouldBe(createdBook.PublishedDate);

        if (!string.IsNullOrWhiteSpace(createdBook.Isbn))
        {
            returnedBook.Isbn.ShouldBe(createdBook.Isbn);
        }
    }

    [Test]
    public async Task Get_Book_By_Non_Existent_Id_Should_Return_404()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var nonExistentId = Guid.NewGuid();

        var response = await booksApiClient.GetBookByIdAsync(nonExistentId);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Get_Book_By_Invalid_Id_Format_Should_Return_400()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var invalidId = "invalid-id";

        var response = await booksApiClient.GetBookByIdAsync(invalidId);

        response.StatusCode.ShouldBe(HttpStatusCode.BadRequest);
    }

    private async Task<BookResponse> CreateBookForTestAsync(BooksApiClient booksApiClient)
    {
        var request = CreateValidBookRequest();

        var createResponse = await booksApiClient.CreateBookAsync(request);
        createResponse.StatusCode.ShouldBe(HttpStatusCode.Created);

        var createdBook = await booksApiClient.ReadResponseAsync<BookResponse>(createResponse);

        createdBook.ShouldNotBeNull();
        createdBook.Id.ShouldNotBe(Guid.Empty);

        RegisterCreatedBook(createdBook.Id);

        return createdBook;
    }

    private static BookCreateRequest CreateValidBookRequest()
    {
        var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];

        return new BookCreateRequest
        {
            Title = $"GetById Test Book {uniqueSuffix}",
            Author = "API Test Author",
            PublishedDate = DateTime.UtcNow,
            Isbn = $"978-7654321-{uniqueSuffix}",
            IsAvailable = true
        };
    }
}