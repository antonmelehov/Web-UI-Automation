using Ehu.UiTests.Core.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Ehu.UiTests.Core.Pages;

public abstract class BasePage
{
    protected IWebDriver Driver { get; }
    protected WebDriverWait Wait { get; }

    protected BasePage(IWebDriver driver)
    {
        Driver = driver;
        Wait = new WebDriverWait(driver, TestSettings.Instance.DefaultWaitTimeout);
    }

    protected IWebElement WaitForVisible(By by)
    {
        return Wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(by);
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
        return Wait.Until(d =>
        {
            try
            {
                var element = d.FindElement(by);
                return element.Displayed && element.Enabled ? element : null;
            }
            catch
            {
                return null;
            }
        });
    }

    protected void JsClick(IWebElement element)
    {
        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].scrollIntoView({block: 'center'});", element);

        ((IJavaScriptExecutor)Driver).ExecuteScript(
            "arguments[0].click();", element);
    }

    protected string GetNormalizedBodyText()
    {
        return Driver.FindElement(By.TagName("body"))
            .Text
            .Replace('\u00A0', ' ')
            .Trim();
    }

    protected void OpenUrl(string url)
    {
        Driver.Navigate().GoToUrl(url);
    }
}