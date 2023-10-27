using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace SaturdayAPI.Core.Types;

public class EventLog
{
    [JsonProperty("logId")]
    public required int LogId { get; set; }

    [JsonProperty("description")]
    public required string Description { get; set; }

    [JsonProperty("memberId")]
    public required string MemberId { get; set; }

    [JsonProperty("action")]
    public required Action Action { get; set; }

    [JsonProperty("gmtCreate")]
    public required DateTimeOffset GmtCreate { get; set; }
}

/// <summary>
/// <c>https://github.com/nbtca/Saturday/blob/v2/util/event-action.go</c>
/// </summary>
[JsonConverter(typeof(ActionConverter))]
public enum Action
{
    Create,
    Accept,
    Cancel,
    Drop,
    Commit,
    AlterCommit,
    Reject,
    Close,
    Update
};

internal class ActionConverter : JsonConverter<Action>
{
    private string ToCamelCase(string str)
    {
        return str.Substring(0, 1).ToLower() + str.Substring(1);
    }

    public override void WriteJson(JsonWriter writer, Action value, JsonSerializer serializer)
    {
        writer.WriteValue(ToCamelCase(value.ToString()));
    }

    public override Action ReadJson(
        JsonReader reader,
        Type objectType,
        Action existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        return Enum.Parse<Action>(reader.Value!.ToString()!, true);
    }
}
