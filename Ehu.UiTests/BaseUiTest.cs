using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace Ehu.UiTests;

public class BaseUiTest
{
    protected IWebDriver Driver = null!;
    protected WebDriverWait Wait = null!;

    [SetUp]
    public void SetUp()
    {
        var options = new ChromeOptions();
        options.AddArgument("--start-maximized");

        Driver = new ChromeDriver(options);
        Wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(15));
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
                return (element.Displayed && element.Enabled) ? element : null;
            }
            catch
            {
                return null;
            }
        });
    }

    protected void AcceptCookiesIfPresent()
    {
        string xpath = "//button[contains(., 'I agree')]";
        
        try
        {
            var buttons = Driver.FindElements(By.XPath(xpath));
            var button = buttons.FirstOrDefault(b => b.Displayed && b.Enabled);

            if (button != null)
            {
                button.Click();
                Thread.Sleep(500);
            }
        }
        catch
        {
        }
    }

    protected void OpenHomePage()
    {
        Driver.Navigate().GoToUrl("https://en.ehuniversity.lt/");
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
        Assert.That(expectedTexts.Any(text => pageText.Contains(text)), Is.True);
    }
}