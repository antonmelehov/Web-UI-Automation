using OpenQA.Selenium;

namespace Ehu.UiTests.XUnit;

public class NavigationTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "UI")]
    [Trait("Category", "Navigation")]
    public void Open_About_Page()
    {
        OpenHomePage();
        AcceptCookiesIfPresent();

        var aboutLink = WaitForClickable(By.XPath("//a[normalize-space()='About']"));
        aboutLink.Click();

        Wait.Until(d => d.Url.Contains("/about/"));

        Assert.Contains("/about/", Driver.Url);
        Assert.Contains("About", Driver.Title);

        var header = WaitForVisible(By.XPath("//h1[contains(normalize-space(), 'About')]"));
        Assert.Contains("About", header.Text);
    }
}