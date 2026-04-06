using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Ehu.UiTests.Core.Drivers;

public static class WebDriverFactory
{
    public static IWebDriver CreateChromeDriver()
    {
        var options = new ChromeOptionsBuilder()
            .StartMaximized()
            .Build();

        return new ChromeDriver(options);
    }
}