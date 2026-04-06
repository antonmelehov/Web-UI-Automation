using Ehu.UiTests.Core.Pages;

namespace Ehu.UiTests.XUnit;

public class SearchTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "UI")]
    [Trait("Category", "Search")]
    public void Search_Study_Programs()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.Search("study programs");

        var contentPage = new ContentPage(Driver);
        contentPage.WaitUntilContainsAnyText("study programs", "bachelor", "master");

        Assert.True(contentPage.ContainsAnyText("study programs", "bachelor", "master"));
    }
}