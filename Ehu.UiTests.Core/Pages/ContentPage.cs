using OpenQA.Selenium;

namespace Ehu.UiTests.Core.Pages;

public class ContentPage : BasePage
{
    public ContentPage(IWebDriver driver) : base(driver)
    {
    }

    public string GetBodyText()
    {
        return GetNormalizedBodyText();
    }

    public bool ContainsText(string expectedText)
    {
        return GetBodyText().Contains(expectedText, StringComparison.OrdinalIgnoreCase);
    }

    public bool ContainsAnyText(params string[] expectedTexts)
    {
        var bodyText = GetBodyText();

        return expectedTexts.Any(text =>
            bodyText.Contains(text, StringComparison.OrdinalIgnoreCase));
    }
}