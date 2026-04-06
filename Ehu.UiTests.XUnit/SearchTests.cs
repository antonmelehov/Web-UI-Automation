using OpenQA.Selenium;
using Ehu.UiTests.Core.Configuration;

namespace Ehu.UiTests.XUnit;

public class SearchTests : BaseUiTest
{
    [Fact]
    [Trait("Category", "UI")]
    [Trait("Category", "Search")]
    public void Search_Study_Programs()
    {
        OpenHomePage();
        AcceptCookiesIfPresent();

        var searchInput = Driver.FindElements(
                By.CssSelector("input[type='search'], input[name='s'], .search-field"))
            .FirstOrDefault(e => e.Displayed && e.Enabled);

        if (searchInput != null)
        {
            searchInput.Clear();
            searchInput.SendKeys("study programs");
            searchInput.SendKeys(Keys.Enter);
        }
        else
        {
            Driver.Navigate().GoToUrl($"{TestSettings.Instance.HomePageUrl}?s=study+programs");
        }

        Wait.Until(d =>
            d.Url.Contains("study+programs") ||
            d.Url.Contains("study%20programs") ||
            d.PageSource.Contains("Study programs"));

        var pageText = GetNormalizedBodyText().ToLower();
        AssertPageContainsAny(pageText, "study programs", "bachelor", "master");
    }
}