using Microsoft.Extensions.Configuration;

namespace Ehu.ApiTests.Configuration;

public static class TestConfiguration
{
    private static readonly IConfigurationRoot Configuration = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
        .Build();

    public static string BaseUrl => GetRequiredValue("Api:BaseUrl");
    public static string TokenUrl => GetRequiredValue("Api:TokenUrl");
    public static string BooksEndpoint => GetRequiredValue("Api:Endpoints:Books");

    public static string ClientId => GetRequiredValue("Api:Auth:ClientId");
    public static string ClientSecret => GetRequiredValue("Api:Auth:ClientSecret");
    public static string Scope => GetRequiredValue("Api:Auth:Scope");
    public static string GrantType => GetRequiredValue("Api:Auth:GrantType");

    private static string GetRequiredValue(string key)
    {
        var value = Configuration[key];

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Configuration value '{key}' is missing or empty.");
        }

        return value;
    }
}