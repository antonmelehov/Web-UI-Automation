using NUnit.Framework;
using OpenQA.Selenium;
using Ehu.UiTests.Core.Configuration;

namespace Ehu.UiTests;

[TestFixture]
[Category("UI")]
[Parallelizable(ParallelScope.Children)]
[FixtureLifeCycle(LifeCycle.InstancePerTestCase)]
public class EhuTests : BaseUiTest
{
    private static readonly string[] ContactInfoTexts =
    {
        "consult@ehu.lt",
        "press@ehu.lt",
        "office@ehu.lt",
        "+370 5 263 9650",
        "Facebook"
    };

    [Test]
    [Category("Navigation")]
    public void Test_01_Open_About_Page()
    {
        OpenHomePage();
        AcceptCookiesIfPresent();

        var aboutLink = WaitForClickable(By.XPath("//a[normalize-space()='About']"));
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
    [Category("Search")]
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
            Driver.Navigate().GoToUrl($"{TestSettings.Instance.HomePageUrl}?s=study+programs");
        }

        Wait.Until(d =>
            d.Url.Contains("study+programs") ||
            d.Url.Contains("study%20programs") ||
            d.PageSource.Contains("Study programs"));

        var pageText = GetNormalizedBodyText().ToLower();
        AssertPageContainsAny(pageText, "study programs", "bachelor", "master");
    }

    [Test]
    [Category("Localization")]
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
            Driver.Navigate().GoToUrl(TestSettings.Instance.LithuanianHomePageUrl);
        }

        Wait.Until(d => d.Url.Contains("lt.ehuniversity.lt"));

        Assert.That(Driver.Url, Does.Contain("lt.ehuniversity.lt"));

        var bodyText = GetNormalizedBodyText().ToLower();
        AssertPageContainsAny(bodyText, "apie mus", "studijos", "europos humanitarinis universitetas");
    }

    [Test]
    [Category("Contacts")]
    [TestCaseSource(nameof(ContactInfoTexts))]
    public void Test_04_Verify_Contact_Info_Contains_Expected_Text(string expectedText)
    {
        Driver.Navigate().GoToUrl(TestSettings.Instance.ContactsPageUrl);

        Wait.Until(d => d.Url.Contains("/contacts"));

        var bodyText = GetNormalizedBodyText();

        Assert.That(bodyText, Does.Contain(expectedText));
    }
}