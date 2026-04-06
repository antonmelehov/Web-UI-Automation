using Ehu.UiTests.Core.Configuration;
using OpenQA.Selenium;

namespace Ehu.UiTests.Core.Pages;

public class HomePage : BasePage
{
    private readonly By _aboutLink = By.XPath("//a[normalize-space()='About']");
    private readonly By _searchInput = By.CssSelector("input[type='search'], input[name='s'], .search-field");
    private readonly By _lithuanianLink = By.XPath("//a[normalize-space()='lt' and contains(@href,'lt.ehuniversity.lt')]");
    private readonly By _cookieButton = By.XPath("//button[contains(., 'I agree')]");

    public HomePage(IWebDriver driver) : base(driver)
    {
    }

    public HomePage Open()
    {
        OpenUrl(TestSettings.Instance.HomePageUrl);
        return this;
    }

    public void AcceptCookiesIfPresent()
    {
        try
        {
            var button = Driver.FindElements(_cookieButton)
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

    public void ClickAbout()
    {
        var aboutLink = WaitForClickable(_aboutLink);
        aboutLink.Click();
    }

    public void Search(string query)
    {
        var searchInput = Driver.FindElements(_searchInput)
            .FirstOrDefault(e => e.Displayed && e.Enabled);

        if (searchInput != null)
        {
            searchInput.Clear();
            searchInput.SendKeys(query);
            searchInput.SendKeys(Keys.Enter);
        }
        else
        {
            OpenUrl($"{TestSettings.Instance.HomePageUrl}?s={Uri.EscapeDataString(query)}");
        }
    }

    public void SwitchToLithuanian()
    {
        var ltLink = Driver.FindElements(_lithuanianLink)
            .FirstOrDefault(e => e.Displayed && e.Enabled);

        if (ltLink != null)
        {
            JsClick(ltLink);
        }
        else
        {
            OpenUrl(TestSettings.Instance.LithuanianHomePageUrl);
        }
    }
}