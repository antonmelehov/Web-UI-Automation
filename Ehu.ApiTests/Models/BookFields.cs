using System.Text.Json.Serialization;

namespace Ehu.ApiTests.Models;

public class BookFields
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = string.Empty;

    [JsonPropertyName("author")]
    public string Author { get; set; } = string.Empty;

    [JsonPropertyName("publishedDate")]
    public DateTime PublishedDate { get; set; }

    [JsonPropertyName("isbn")]
    public string? Isbn { get; set; }

    [JsonPropertyName("isAvailable")]
    public bool? IsAvailable { get; set; }
}