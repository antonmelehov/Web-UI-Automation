using System.Net;
using Ehu.ApiTests.Clients;
using Ehu.ApiTests.Models;
using Shouldly;

namespace Ehu.ApiTests.Tests;

[TestFixture]
[Category("API")]
[Category("Books")]
[Category("Update")]
public class UpdateBookTests : BaseApiTest
{
    [Test]
    public async Task Update_Existing_Book_With_Valid_Data_Should_Succeed()
    {
        var booksApiClient = new BooksApiClient(HttpClient);

        var createdBook = await CreateBookForTestAsync(booksApiClient);
        var updateRequest = CreateUpdateRequest();

        var updateResponse = await booksApiClient.UpdateBookAsync(createdBook.Id, updateRequest);

        updateResponse.StatusCode.ShouldBeOneOf(HttpStatusCode.OK, HttpStatusCode.NoContent);

        var getResponse = await booksApiClient.GetBookByIdAsync(createdBook.Id);
        getResponse.StatusCode.ShouldBe(HttpStatusCode.OK);

        var updatedBook = await booksApiClient.ReadResponseAsync<BookResponse>(getResponse);

        updatedBook.ShouldNotBeNull();
        updatedBook.Id.ShouldBe(createdBook.Id);
        updatedBook.Title.ShouldBe(updateRequest.Title);
        updatedBook.Author.ShouldBe(updateRequest.Author);
        updatedBook.PublishedDate.ShouldBe(updateRequest.PublishedDate);
    }

    [Test]
    public async Task Update_Book_With_Non_Existent_Id_Should_Return_404()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var nonExistentId = Guid.NewGuid();
        var updateRequest = CreateUpdateRequest();

        var response = await booksApiClient.UpdateBookAsync(nonExistentId, updateRequest);

        response.StatusCode.ShouldBe(HttpStatusCode.NotFound);
    }

    [Test]
    public async Task Update_Book_With_Invalid_Id_Format_Should_Return_400()
    {
        var booksApiClient = new BooksApiClient(HttpClient);
        var invalidId = "invalid-id";
        var updateRequest = CreateUpdateRequest();

        var response = await booksApiClient.UpdateBookAsync(invalidId, updateRequest);

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
            Title = $"Update Test Book {uniqueSuffix}",
            Author = "Original Author",
            PublishedDate = DateTime.UtcNow,
            Isbn = $"978-1111111-{uniqueSuffix}",
            IsAvailable = true
        };
    }

    private static BookUpdateRequest CreateUpdateRequest()
    {
        var uniqueSuffix = Guid.NewGuid().ToString("N")[..8];

        return new BookUpdateRequest
        {
            Title = $"Updated Book {uniqueSuffix}",
            Author = "Updated Author",
            PublishedDate = DateTime.UtcNow.AddDays(1),
            Isbn = $"978-2222222-{uniqueSuffix}",
            IsAvailable = false
        };
    }
}