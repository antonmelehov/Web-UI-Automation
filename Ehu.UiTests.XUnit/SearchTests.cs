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

        Wait.Until(d =>
            d.Url.Contains("study+programs") ||
            d.Url.Contains("study%20programs") ||
            d.PageSource.Contains("Study programs"));

        var contentPage = new ContentPage(Driver);

        Assert.True(contentPage.ContainsAnyText("study programs", "bachelor", "master"));
    }
}