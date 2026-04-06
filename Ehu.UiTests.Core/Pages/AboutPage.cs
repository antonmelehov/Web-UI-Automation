using OpenQA.Selenium;

namespace Ehu.UiTests.Core.Pages;

public class AboutPage : BasePage
{
    private readonly By _header = By.XPath("//h1[contains(normalize-space(), 'About')]");

    public AboutPage(IWebDriver driver) : base(driver)
    {
    }

    public bool IsOpened()
    {
        return Driver.Url.Contains("/about/");
    }

    public string GetHeaderText()
    {
        return WaitForVisible(_header).Text;
    }

    public bool HeaderContains(string expectedText)
    {
        return GetHeaderText().Contains(expectedText);
    }
}