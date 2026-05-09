using System.Text.Json.Serialization;

namespace Ehu.ApiTests.Models;

public class BookResponse : BookFields
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}