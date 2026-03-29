using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Ehu.UiTests.XUnit;

public abstract class BaseUiTest : IDisposable
{
    protected const string HomePageUrl = "https://en.ehuniversity.lt/";
    protected const string LithuanianHomePageUrl = "https://lt.ehuniversity.lt/";
    protected const string ContactsPageUrl = "https://en.ehuniversity.lt/contacts/";

    protected IWebDriver Driver { get; }
    protected WebDriverWait Wait { get; }

    private static readonly TimeSpan DefaultWaitTimeout = TimeSpan.FromSeconds(15);

    protected BaseUiTest()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        Driver = new ChromeDriver(options);
        Wait = new WebDriverWait(Driver, DefaultWaitTimeout);
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

    protected IWebElement WaitForVisible(By by)
    {
        return Wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(by);
                return element.Displayed ? element : null;
            }
            catch
            {
                return null;
            }
        });
    }

    protected IWebElement WaitForClickable(By by)
    {
        return Wait.Until(driver =>
        {
            try
            {
                var element = driver.FindElement(by);
                return element.Displayed && element.Enabled ? element : null;
            }
            catch
            {
                return null;
            }
        });
    }

    protected void AcceptCookiesIfPresent()
    {
        try
        {
            var button = Driver.FindElements(By.XPath("//button[contains(., 'I agree')]"))
                .FirstOrDefault(b => b.Displayed && b.Enabled);

            if (button == null)
                return;

            button.Click();
            Thread.Sleep(500);
        }
        catch
        {
        }
    }

    protected void OpenHomePage()
    {
        Driver.Navigate().GoToUrl(HomePageUrl);
    }

    protected string GetNormalizedBodyText()
    {
        return Driver.FindElement(By.TagName("body"))
            .Text
            .Replace('\u00A0', ' ')
            .Trim();
    }

    protected void JsClick(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].scrollIntoView({block: 'center'});", element);

        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].click();", element);
    }

    protected void AssertPageContainsAny(string pageText, params string[] expectedTexts)
    {
        Assert.Contains(expectedTexts, text => pageText.Contains(text));
    }
}