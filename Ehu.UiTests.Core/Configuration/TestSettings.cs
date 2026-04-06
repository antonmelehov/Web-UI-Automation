namespace Ehu.UiTests.Core.Configuration;

public sealed class TestSettings
{
    private static readonly Lazy<TestSettings> _instance = new(() => new TestSettings());

    public static TestSettings Instance => _instance.Value;

    public string HomePageUrl => "https://en.ehuniversity.lt/";
    public string LithuanianHomePageUrl => "https://lt.ehuniversity.lt/";
    public string ContactsPageUrl => "https://en.ehuniversity.lt/contacts/";
    public TimeSpan DefaultWaitTimeout => TimeSpan.FromSeconds(15);

    private TestSettings()
    {
    }
}