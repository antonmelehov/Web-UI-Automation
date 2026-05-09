using System.Net;
using Ehu.ApiTests.Clients;
using Ehu.ApiTests.Models;
using NUnit.Framework;
using Shouldly;

namespace Ehu.ApiTests.Tests;

[TestFixture]
[Category("API")]
[Category("Books")]
[Category("Delete")]
public class DeleteBookTests : BaseApiTest
{
    [Test]
    public async Task Delete_Book_By_Valid_Id_Should_Return_204_And_Book_Should_Not_Be_Retrievable()
    {
        var booksApiClient = new BooksApiClient(HttpClient);

        var createdBook = await CreateBookForTestAsync(booksApiClient);

        var deleteResponse = await booksApiClient.DeleteBookAsync(createdBook.Id);

        deleteResponse.StatusCode.ShouldBe(HttpStatusCode.NoContent);

        var getResponse = await booksApiClient.GetBookByIdAsync(createdBook.Id);

        getResponse.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_Book_By_Non_Existent_Id_Should_Return_404()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var nonExistentId = Guid.NewGuid();

        var response = await booksApiClient.DeleteBookAsync(nonExistentId);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Delete_Book_By_Invalid_Id_Format_Should_Return_400()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var invalidId = "invalid-id";

        var response = await booksApiClient.DeleteBookAsync(invalidId);

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

        return createdBook;
    }

    private static BookCreateRequest CreateValidBookRequest()
    {
        var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];

        return new BookCreateRequest
        {
            Title = $"Delete Test Book {uniqueSuffix}",
            Author = "API Test Author",
            PublishedDate = DateTime.UtcNow,
            Isbn = $"978-3333333-{uniqueSuffix}",
            IsAvailable = true
        };
    }
}