using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using Ehu.UiTests.Core.Configuration;
using Ehu.UiTests.Core.Drivers;

namespace Ehu.UiTests;

public class BaseUiTest
{
    protected IWebDriver Driver = null!;

    [SetUp]
    public void SetUp()
    {
        Driver = WebDriverFactory.CreateChromeDriver();
    }

    [TearDown]
    public void TearDown()
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