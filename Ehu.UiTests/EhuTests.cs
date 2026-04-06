using NUnit.Framework;
using OpenQA.Selenium;
using Ehu.UiTests.Core.Configuration;
using Ehu.UiTests.Core.Pages;

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
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.ClickAbout();

        var aboutPage = new AboutPage(Driver);

        Assert.Multiple(() =>
        {
            Assert.That(aboutPage.IsOpened(), Is.True);
            Assert.That(Driver.Title, Does.Contain("About"));
            Assert.That(aboutPage.HeaderContains("About"), Is.True);
        });
    }

    [Test]
    [Category("Search")]
    public void Test_02_Search_Study_Programs()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.Search("study programs");

        Wait.Until(d =>
            d.Url.Contains("study+programs") ||
            d.Url.Contains("study%20programs") ||
            d.PageSource.Contains("Study programs"));

        var contentPage = new ContentPage(Driver);

        Assert.That(contentPage.ContainsAnyText("study programs", "bachelor", "master"), Is.True);
    }

    [Test]
    [Category("Localization")]
    public void Test_03_Change_Language_To_Lithuanian()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.SwitchToLithuanian();

        Wait.Until(d => d.Url.Contains("lt.ehuniversity.lt"));

        Assert.That(Driver.Url, Does.Contain("lt.ehuniversity.lt"));

        var contentPage = new ContentPage(Driver);

        Assert.That(contentPage.ContainsAnyText("apie mus", "studijos", "europos humanitarinis universitetas"), Is.True);
    }

    [Test]
    [Category("Contacts")]
    [TestCaseSource(nameof(ContactInfoTexts))]
    public void Test_04_Verify_Contact_Info_Contains_Expected_Text(string expectedText)
    {
        var contactsPage = new ContactsPage(Driver).Open();

        Wait.Until(d => contactsPage.IsOpened());

        Assert.That(contactsPage.ContainsText(expectedText), Is.True);
    }
}