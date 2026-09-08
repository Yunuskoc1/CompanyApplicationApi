using System.Text.Json.Serialization;

namespace CompanyApplicationApi.Data;

public class SqlExportRoot
{
    [JsonPropertyName("type")]
    public string? Type { get; set; }

    [JsonPropertyName("name")]
    public string? Name { get; set; }

    [JsonPropertyName("data")]
    public List<SqlExportDataRow>? Data { get; set; }
}

public class SqlExportDataRow
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("il_id")]
    public string? IlId { get; set; }

    [JsonPropertyName("ilid")]
    public string? IlIdAlt { get; set; }
}