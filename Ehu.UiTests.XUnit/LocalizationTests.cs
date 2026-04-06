using Ehu.UiTests.Core.Pages;

namespace Ehu.UiTests.XUnit;

public class LocalizationTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "UI")]
    [Trait("Category", "Localization")]
    public void Change_Language_To_Lithuanian()
    {
        var homePage = new HomePage(Driver).Open();
        homePage.AcceptCookiesIfPresent();
        homePage.SwitchToLithuanian();

        var contentPage = new ContentPage(Driver);
        contentPage.WaitUntilUrlContains("lt.ehuniversity.lt");

        Assert.Contains("lt.ehuniversity.lt", Driver.Url);
        Assert.True(contentPage.ContainsAnyText("apie mus", "studijos", "europos humanitarinis universitetas"));
    }
}