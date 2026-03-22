using NUnit.Framework;
using OpenQA.Selenium;

namespace Ehu.UiTests;

[TestFixture]
public class EhuTests : BaseUiTest
{
    [Test]
    public void Test_01_Open_About_Page()
    {
        OpenHomePage();
        AcceptCookiesIfPresent();

        var aboutLink = WaitForClickable(
            By.XPath("//a[normalize-space()='About']"));

        aboutLink.Click();

        Wait.Until(d => d.Url.Contains("/about/"));

        Assert.Multiple(() =>
        {
            Assert.That(Driver.Url, Does.Contain("/about/"));
            Assert.That(Driver.Title, Does.Contain("About"));
        });

        var header = WaitForVisible(By.XPath("//h1[contains(normalize-space(), 'About')]"));
        Assert.That(header.Text, Does.Contain("About"));
    }

    [Test]
    public void Test_02_Search_Study_Programs()
    {
        OpenHomePage();
        AcceptCookiesIfPresent();

        var searchInput = Driver.FindElements(
                By.CssSelector("input[type='search'], input[name='s'], .search-field"))
            .FirstOrDefault(e => e.Displayed && e.Enabled);

        if (searchInput != null)
        {
            searchInput.Clear();
            searchInput.SendKeys("study programs");
            searchInput.SendKeys(Keys.Enter);
        }
        else
        {
            Driver.Navigate().GoToUrl("https://en.ehuniversity.lt/?s=study+programs");
        }

        Wait.Until(d =>
            d.Url.Contains("study+programs") ||
            d.Url.Contains("study%20programs") ||
            d.PageSource.Contains("Study programs"));

        var pageText = GetNormalizedBodyText().ToLower();

        AssertPageContainsAny(pageText, "study programs", "bachelor", "master");
    }

    [Test]
    public void Test_03_Change_Language_To_Lithuanian()
    {
        OpenHomePage();
        AcceptCookiesIfPresent();

        var ltLink = Driver.FindElements(
                By.XPath("//a[normalize-space()='lt' and contains(@href,'lt.ehuniversity.lt')]"))
            .FirstOrDefault(e => e.Displayed && e.Enabled);

        if (ltLink != null)
        {
            JsClick(ltLink);
        }
        else
        {
            Driver.Navigate().GoToUrl("https://lt.ehuniversity.lt/");
        }

        Wait.Until(d => d.Url.Contains("lt.ehuniversity.lt"));

        Assert.That(Driver.Url, Does.Contain("lt.ehuniversity.lt"));

        var bodyText = GetNormalizedBodyText().ToLower();

        AssertPageContainsAny(bodyText, "apie mus", "studijos", "europos humanitarinis universitetas");
    }

    [Test]
    public void Test_04_Verify_Contact_Info_Is_Displayed()
    {
        Driver.Navigate().GoToUrl("https://en.ehuniversity.lt/contacts/");

        Wait.Until(d => d.Url.Contains("/contacts"));

        var bodyText = GetNormalizedBodyText();

        Assert.Multiple(() =>
        {
            Assert.That(bodyText, Does.Contain("consult@ehu.lt"));
            Assert.That(bodyText, Does.Contain("press@ehu.lt"));
            Assert.That(bodyText, Does.Contain("office@ehu.lt"));
            Assert.That(bodyText, Does.Contain("+370 5 263 9650"));
            Assert.That(bodyText, Does.Contain("Facebook"));
        });
    }
}