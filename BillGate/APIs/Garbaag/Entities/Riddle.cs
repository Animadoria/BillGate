using System.Text.Json.Serialization;

namespace BillGate.APIs.Garbaag.Entities;

public class Riddle
{
    [JsonPropertyName("title")] public string Title { get; set; } = "";

    [JsonPropertyName("question")] public string Question { get; set; } = "";

    [JsonPropertyName("answer")] public string Answer { get; set; } = "";
}