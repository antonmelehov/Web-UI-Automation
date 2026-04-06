using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using Ehu.UiTests.Core.Configuration;
using Ehu.UiTests.Core.Drivers;

namespace Ehu.UiTests.XUnit;

public abstract class BaseUiTest : IDisposable
{
    protected IWebDriver Driver { get; }
    protected WebDriverWait Wait { get; }

    protected BaseUiTest()
    {
        Driver = WebDriverFactory.CreateChromeDriver();
        Wait = new WebDriverWait(Driver, TestSettings.Instance.DefaultWaitTimeout);
    }

    public void Dispose()
    {
        try
        {
            Driver.Quit();
            Driver.Dispose();
        }
        catch
        {
        }
    }
}