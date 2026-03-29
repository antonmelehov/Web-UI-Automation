namespace Ehu.UiTests.XUnit;

public class ContactsTests : BaseUiTest
{
    public static IEnumerable<object[]> ContactInfoTexts =>
        new List<object[]>
        {
            new object[] { "consult@ehu.lt" },
            new object[] { "press@ehu.lt" },
            new object[] { "office@ehu.lt" },
            new object[] { "+370 5 263 9650" },
            new object[] { "Facebook" }
        };

    [Theory]
    [MemberData(nameof(ContactInfoTexts))]
    [Trait("Category", "UI")]
    [Trait("Category", "Contacts")]
    public void Contact_Info_Contains_Expected_Text(string expectedText)
    {
        Driver.Navigate().GoToUrl(ContactsPageUrl);

        Wait.Until(d => d.Url.Contains("/contacts"));

        var bodyText = GetNormalizedBodyText();
        Assert.Contains(expectedText, bodyText);
    }
}