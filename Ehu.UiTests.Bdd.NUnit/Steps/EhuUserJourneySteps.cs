using Ehu.UiTests.Core.Pages;
using OpenQA.Selenium;
using Reqnroll;

namespace Ehu.UiTests.Bdd.NUnit.Steps;

[Binding]
public class EhuUserJourneySteps
{
    private readonly ScenarioContext _scenarioContext;

    public EhuUserJourneySteps(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    private IWebDriver Driver => (IWebDriver)_scenarioContext["Driver"];

    private HomePage HomePage => new(Driver);
    private AboutPage AboutPage => new(Driver);
    private ContentPage ContentPage => new(Driver);
    private ContactsPage ContactsPage => new(Driver);

    [Given("the user opens the EHU home page")]
    [When("the user opens the EHU home page")]
    public void OpenTheEhuHomePage()
    {
        HomePage.Open();
    }

    [Given("accepts cookies if the banner is shown")]
    public void AcceptCookiesIfTheBannerIsShown()
    {
        HomePage.AcceptCookiesIfPresent();
    }

    [When("the user navigates to the About page")]
    public void NavigateToTheAboutPage()
    {
        HomePage.ClickAbout();
    }

    [Then("the About page should be opened")]
    public void TheAboutPageShouldBeOpened()
    {
        AboutPage.WaitUntilOpened();

        Assert.That(AboutPage.IsOpened(), Is.True);
    }

    [Then("the About page header should contain {string}")]
    public void TheAboutPageHeaderShouldContain(string expectedText)
    {
        Assert.That(AboutPage.HeaderContains(expectedText), Is.True);
    }

    [When("searches for {string}")]
    public void SearchesFor(string query)
    {
        HomePage.Search(query);
    }

    [Then("the user should see content related to study programs")]
    public void TheUserShouldSeeContentRelatedToStudyPrograms()
    {
        ContentPage.WaitUntilContainsAnyText("study programs", "bachelor", "master");

        Assert.That(
            ContentPage.ContainsAnyText("study programs", "bachelor", "master"),
            Is.True);
    }

    [When("the user switches the site language to Lithuanian")]
    public void SwitchTheSiteLanguageToLithuanian()
    {
        HomePage.SwitchToLithuanian();
    }

    [Then("the Lithuanian version of the site should be opened")]
    public void TheLithuanianVersionOfTheSiteShouldBeOpened()
    {
        ContentPage.WaitUntilUrlContains("lt.ehuniversity.lt");

        Assert.That(Driver.Url, Does.Contain("lt.ehuniversity.lt"));
    }

    [Then("the user should see Lithuanian content on the page")]
    public void TheUserShouldSeeLithuanianContentOnThePage()
    {
        Assert.That(
            ContentPage.ContainsAnyText("apie mus", "studijos", "europos humanitarinis universitetas"),
            Is.True);
    }

    [When("the user opens the contacts page")]
    public void TheUserOpensTheContactsPage()
    {
        ContactsPage.Open();
    }

    [Then("the contacts page should be opened")]
    public void TheContactsPageShouldBeOpened()
    {
        ContactsPage.WaitUntilOpened();

        Assert.That(ContactsPage.IsOpened(), Is.True);
    }

    [Then("the page should contain contact text {string}")]
    public void ThePageShouldContainContactText(string expectedText)
    {
        Assert.That(ContactsPage.ContainsText(expectedText), Is.True);
    }
}