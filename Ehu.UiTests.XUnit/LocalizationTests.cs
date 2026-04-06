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

        Wait.Until(d => d.Url.Contains("lt.ehuniversity.lt"));

        Assert.Contains("lt.ehuniversity.lt", Driver.Url);

        var contentPage = new ContentPage(Driver);

        Assert.True(contentPage.ContainsAnyText("apie mus", "studijos", "europos humanitarinis universitetas"));
    }
}