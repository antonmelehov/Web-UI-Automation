using Ehu.UiTests.Core.Pages;

namespace Ehu.UiTests.XUnit;

public class NavigationTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "UI")]
    [Trait("Category", "Navigation")]
    public void Open_About_Page()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.ClickAbout();

        var aboutPage = new AboutPage(Driver);
        aboutPage.WaitUntilOpened();

        Assert.Multiple(() =>
        {
            Assert.True(aboutPage.IsOpened());
            Assert.Contains("About", Driver.Title);
            Assert.True(aboutPage.HeaderContains("About"));
        });
    }
}