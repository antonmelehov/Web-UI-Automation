using Ehu.UiTests.Core.Drivers;
using OpenQA.Selenium;
using Reqnroll;

namespace Ehu.UiTests.Bdd.NUnit.Hooks;

[Binding]
public class BrowserHooks
{
    private readonly ScenarioContext _scenarioContext;

    public BrowserHooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var driver = WebDriverFactory.CreateChromeDriver();
        _scenarioContext["Driver"] = driver;

        TestContext.Progress.WriteLine($"Starting scenario: {_scenarioContext.ScenarioInfo.Title}");
    }

    [AfterScenario]
    public void AfterScenario()
    {
        if (!_scenarioContext.TryGetValue("Driver", out IWebDriver? driver) || driver == null)
            return;

        try
        {
            if (_scenarioContext.TestError != null)
            {
                TestContext.Progress.WriteLine($"Scenario failed: {_scenarioContext.ScenarioInfo.Title}");
                TestContext.Progress.WriteLine($"Error: {_scenarioContext.TestError.Message}");
                TestContext.Progress.WriteLine($"Current URL: {driver.Url}");
                TestContext.Progress.WriteLine($"Page title: {driver.Title}");

                SaveScreenshot(driver);
            }
            else
            {
                TestContext.Progress.WriteLine($"Scenario passed: {_scenarioContext.ScenarioInfo.Title}");
            }
        }
        finally
        {
            try
            {
                driver.Quit();
                driver.Dispose();
            }
            catch
            {
            }
        }
    }

    private void SaveScreenshot(IWebDriver driver)
    {
        if (driver is not ITakesScreenshot screenshotDriver)
            return;

        var screenshotsDir = Path.Combine(
            TestContext.CurrentContext.WorkDirectory,
            "TestResults",
            "Screenshots");

        Directory.CreateDirectory(screenshotsDir);

        var fileName =
            $"{SanitizeFileName(_scenarioContext.ScenarioInfo.Title)}_{DateTime.Now:yyyyMMdd_HHmmss}.png";

        var fullPath = Path.Combine(screenshotsDir, fileName);

        var screenshot = screenshotDriver.GetScreenshot();
        screenshot.SaveAsFile(fullPath);

        TestContext.Progress.WriteLine($"Screenshot saved: {fullPath}");
        TestContext.AddTestAttachment(fullPath, "Failure screenshot");
    }

    private static string SanitizeFileName(string fileName)
    {
        foreach (var invalidChar in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalidChar, '_');
        }

        return fileName;
    }
}