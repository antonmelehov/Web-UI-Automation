using System.Net;
using Ehu.ApiTests.Clients;
using Ehu.ApiTests.Models;
using Shouldly;

namespace Ehu.ApiTests.Tests;

[TestFixture]
[Category("API")]
[Category("Books")]
[Category("GetAll")]
public class GetAllBooksTests : BaseApiTest
{
    [Test]
    public async Task Get_All_Books_Should_Return_List_Of_Books()
    {
        var booksApiClient = new BooksApiClient(HttpClient);

        var createdBook = await CreateBookForTestAsync(booksApiClient);

        var response = await booksApiClient.GetAllBooksAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var books = await booksApiClient.ReadResponseAsync<List<BookResponse>>(response);

        books.ShouldNotBeNull();
        books.Count.ShouldBeGreaterThan(0);

        books.Any(book => book.Id == createdBook.Id).ShouldBeTrue();
    }

    [Test]
    public async Task Get_All_Books_Should_Return_Required_Book_Fields()
    {
        var booksApiClient = new BooksApiClient(HttpClient);

        var createdBook = await CreateBookForTestAsync(booksApiClient);

        var response = await booksApiClient.GetAllBooksAsync();

        response.StatusCode.ShouldBe(HttpStatusCode.OK);

        var books = await booksApiClient.ReadResponseAsync<List<BookResponse>>(response);

        books.ShouldNotBeNull();
        books.Count.ShouldBeGreaterThan(0);

        var returnedBook = books.FirstOrDefault(book => book.Id == createdBook.Id);

        returnedBook.ShouldNotBeNull();
        returnedBook.Title.ShouldNotBeNullOrWhiteSpace();
        returnedBook.Author.ShouldNotBeNullOrWhiteSpace();
        returnedBook.PublishedDate.ShouldNotBe(default(DateTime));
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
            Title = $"GetAll Test Book {uniqueSuffix}",
            Author = "API Test Author",
            PublishedDate = DateTime.UtcNow,
            Isbn = $"978-1234567-{uniqueSuffix}",
            IsAvailable = true
        };
    }
}