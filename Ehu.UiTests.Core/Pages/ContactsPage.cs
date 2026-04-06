using Ehu.UiTests.Core.Configuration;
using OpenQA.Selenium;

namespace Ehu.UiTests.Core.Pages;

public class ContactsPage : BasePage
{
    public ContactsPage(IWebDriver driver) : base(driver)
    {
    }

    public ContactsPage Open()
    {
        OpenUrl(TestSettings.Instance.ContactsPageUrl);
        return this;
    }

    public void WaitUntilOpened()
    {
        WaitUntilUrlContains("/contacts");
    }

    public bool IsOpened()
    {
        return Driver.Url.Contains("/contacts");
    }

    public string GetBodyText()
    {
        return GetNormalizedBodyText();
    }

    public bool ContainsText(string expectedText)
    {
        return GetBodyText().Contains(expectedText);
    }
}