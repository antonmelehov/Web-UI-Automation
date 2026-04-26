using Allure.NUnit;
using Ehu.UiTests.Core.Drivers;
using Ehu.UiTests.Logging;
using NUnit.Framework;
using NUnit.Framework.Interfaces;
using OpenQA.Selenium;
using Serilog;
using System.Diagnostics;

namespace Ehu.UiTests;

[AllureNUnit]
public class BaseUiTest
{
    protected IWebDriver Driver = null!;
    private Stopwatch _stopwatch = null!;

    [SetUp]
    public void SetUp()
    {
        TestLogger.Configure();

        var testName = TestContext.CurrentContext.Test.Name;
        Log.Information("Starting test: {TestName}", testName);

        _stopwatch = Stopwatch.StartNew();

        try
        {
            Driver = WebDriverFactory.CreateChromeDriver();
            Log.Debug("WebDriver created successfully for test: {TestName}", testName);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to create WebDriver for test: {TestName}", testName);
            throw;
        }
    }

    [TearDown]
    public void TearDown()
    {
        _stopwatch.Stop();

        var testName = TestContext.CurrentContext.Test.Name;
        var result = TestContext.CurrentContext.Result;
        var status = result.Outcome.Status;
        var duration = _stopwatch.Elapsed;

        switch (status)
        {
            case TestStatus.Passed:
                Log.Information(
                    "Test passed: {TestName}. Duration: {Duration}s",
                    testName,
                    duration);
                break;

            case TestStatus.Skipped:
            case TestStatus.Inconclusive:
                Log.Warning(
                    "Test skipped or inconclusive: {TestName}. Status: {Status}. Duration: {Duration}s",
                    testName,
                    status,
                    duration);
                break;

            case TestStatus.Failed:
                Log.Error(
                    "Test failed: {TestName}. Duration: {Duration}s. Message: {Message}",
                    testName,
                    duration,
                    result.Message);
                break;

            default:
                Log.Warning(
                    "Test finished with unexpected status: {TestName}. Status: {Status}. Duration: {Duration}s",
                    testName,
                    status,
                    duration);
                break;
        }

        try
        {
            Driver?.Quit();
            Driver?.Dispose();
            Log.Debug("WebDriver disposed for test: {TestName}", testName);
        }
        catch (Exception ex)
        {
            Log.Warning(ex, "Error during WebDriver cleanup for test: {TestName}", testName);
        }
    }
}