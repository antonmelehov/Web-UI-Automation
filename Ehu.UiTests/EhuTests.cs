using NUnit.Framework;
using Shouldly;
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
        aboutPage.WaitUntilOpened();

        aboutPage.IsOpened().ShouldBeTrue();
        Driver.Title.ShouldContain("About");
        aboutPage.HeaderContains("About").ShouldBeTrue();
    }

    [Test]
    [Category("Search")]
    public void Test_02_Search_Study_Programs()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.Search("study programs");

        var contentPage = new ContentPage(Driver);
        contentPage.WaitUntilContainsAnyText("study programs", "bachelor", "master");

        contentPage.ContainsAnyText("study programs", "bachelor", "master").ShouldBeTrue();
    }

    [Test]
    [Category("Localization")]
    public void Test_03_Change_Language_To_Lithuanian()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.SwitchToLithuanian();
        
        var contentPage = new ContentPage(Driver);
        contentPage.WaitUntilUrlContains("lt.ehuniversity.lt");

        Driver.Url.ShouldContain("lt.ehuniversity.lt");
        contentPage.ContainsAnyText("apie mus", "studijos", "europos humanitarinis universitetas").ShouldBeTrue();
    }

    [Test]
    [Category("Contacts")]
    [TestCaseSource(nameof(ContactInfoTexts))]
    public void Test_04_Verify_Contact_Info_Contains_Expected_Text(string expectedText)
    {
        var contactsPage = new ContactsPage(Driver).Open();
        contactsPage.WaitUntilOpened();

        contactsPage.ContainsText(expectedText).ShouldBeTrue();
    }
}