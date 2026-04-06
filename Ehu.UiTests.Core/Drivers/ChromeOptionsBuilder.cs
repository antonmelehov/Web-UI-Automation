using OpenQA.Selenium.Chrome;

namespace Ehu.UiTests.Core.Drivers;

public class ChromeOptionsBuilder
{
    private readonly ChromeOptions _options = new();

    public ChromeOptionsBuilder StartMaximized()
    {
        _options.AddArgument("--start-maximized");
        return this;
    }

    public ChromeOptionsBuilder Headless()
    {
        _options.AddArgument("--headless=new");
        return this;
    }

    public ChromeOptionsBuilder DisableNotifications()
    {
        _options.AddArgument("--disable-notifications");
        return this;
    }

    public ChromeOptionsBuilder DisableGpu()
    {
        _options.AddArgument("--disable-gpu");
        return this;
    }

    public ChromeOptions Build()
    {
        return _options;
    }
}