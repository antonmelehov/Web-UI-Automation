using OpenQA.Selenium;

namespace Ehu.UiTests.XUnit;

public class LocalizationTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "UI")]
    [Trait("Category", "Localization")]
    public void Change_Language_To_Lithuanian()
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
            Driver.Navigate().GoToUrl(LithuanianHomePageUrl);
        }

        Wait.Until(d => d.Url.Contains("lt.ehuniversity.lt"));

        Assert.Contains("lt.ehuniversity.lt", Driver.Url);

        var bodyText = GetNormalizedBodyText().ToLower();
        AssertPageContainsAny(bodyText, "apie mus", "studijos", "europos humanitarinis universitetas");
    }
}