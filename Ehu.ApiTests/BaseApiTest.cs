using System.Diagnostics;
using System.Net.Http.Headers;
using Allure.NUnit;
using Ehu.ApiTests.Clients;
using Ehu.ApiTests.Configuration;
using Ehu.ApiTests.Logging;
using NUnit.Framework.Interfaces;
using Serilog;

namespace Ehu.ApiTests;

[AllureNUnit]
public abstract class BaseApiTest
{
    protected HttpClient HttpClient = null!;
    protected string AccessToken = string.Empty;

    private Stopwatch _stopwatch = null!;
    private readonly List<Guid> _createdBookIds = new();

    [SetUp]
    public async Task SetUp()
    {
        TestLogger.Configure();

        var testName = TestContext.CurrentContext.Test.Name;
        Log.Information("Starting API test: {TestName}", testName);

        _stopwatch = Stopwatch.StartNew();

        try
        {
            using var authClient = new AuthClient();
            AccessToken = await authClient.GetAccessTokenAsync();

            HttpClient = new HttpClient
            {
                BaseAddress = new Uri(TestConfiguration.BaseUrl)
            };

            HttpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", AccessToken);

            HttpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            Log.Debug("HttpClient initialized successfully for test: {TestName}", testName);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to initialize API test infrastructure for test: {TestName}", testName);
            throw;
        }
    }

    [TearDown]
    public async Task TearDown()
    {
        _stopwatch.Stop();

        await CleanupCreatedBooksAsync();

        var testName = TestContext.CurrentContext.Test.Name;
        var result = TestContext.CurrentContext.Result;
        var status = result.Outcome.Status;
        var duration = _stopwatch.Elapsed;

        switch (status)
        {
            case TestStatus.Passed:
                Log.Information(
                    "API test passed: {TestName}. Duration: {Duration}",
                    testName,
                    duration);
                break;

            case TestStatus.Skipped:
            case TestStatus.Inconclusive:
                Log.Warning(
                    "API test skipped or inconclusive: {TestName}. Status: {Status}. Duration: {Duration}",
                    testName,
                    status,
                    duration);
                break;

            case TestStatus.Failed:
                Log.Error(
                    "API test failed: {TestName}. Duration: {Duration}. Message: {Message}",
                    testName,
                    duration,
                    result.Message);
                break;

            default:
                Log.Warning(
                    "API test finished with unexpected status: {TestName}. Status: {Status}. Duration: {Duration}",
                    testName,
                    status,
                    duration);
                break;
        }

        HttpClient?.Dispose();
    }

    protected string BooksEndpoint => TestConfiguration.BooksEndpoint;

    protected void RegisterCreatedBook(Guid bookId)
    {
        if (bookId != Guid.Empty)
        {
            _createdBookIds.Add(bookId);
            Log.Debug("Registered created book for cleanup: {BookId}", bookId);
        }
    }

    protected string BuildBookByIdEndpoint(Guid id)
    {
        return $"{BooksEndpoint}/{id}";
    }

    private async Task CleanupCreatedBooksAsync()
    {
        if (HttpClient == null || _createdBookIds.Count == 0)
            return;

        foreach (var bookId in _createdBookIds.Distinct())
        {
            try
            {
                var response = await HttpClient.DeleteAsync(BuildBookByIdEndpoint(bookId));

                Log.Debug(
                    "Cleanup delete request for book {BookId}. Status code: {StatusCode}",
                    bookId,
                    (int)response.StatusCode);
            }
            catch (Exception ex)
            {
                Log.Warning(ex, "Failed to cleanup book with id {BookId}", bookId);
            }
        }

        _createdBookIds.Clear();
    }
}