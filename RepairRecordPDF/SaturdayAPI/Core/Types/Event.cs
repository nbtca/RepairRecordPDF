using Newtonsoft.Json;

namespace SaturdayAPI.Core.Types;

public class EventInfo
{
    [JsonProperty("eventId")]
    public required int EventId { get; set; }

    [JsonProperty("clientId")]
    public required int ClientId { get; set; }

    [JsonProperty("model")]
    public required string Model { get; set; }

    [JsonProperty("problem")]
    public required string Problem { get; set; }

    [JsonProperty("member")]
    public required ClosedBy Member { get; set; }

    [JsonProperty("closedBy")]
    public required ClosedBy ClosedBy { get; set; }

    [JsonProperty("status")]
    public required Status Status { get; set; }

    [JsonProperty("logs")]
    public EventLog[]? Logs { get; set; }

    [JsonProperty("gmtCreate")]
    public required DateTimeOffset GmtCreate { get; set; }

    [JsonProperty("gmtModified")]
    public required DateTimeOffset GmtModified { get; set; }
}

public class EventLog
{
    [JsonProperty("logId")]
    public required int LogId { get; set; }

    [JsonProperty("description")]
    public required string Description { get; set; }

    [JsonProperty("memberId")]
    public required string MemberId { get; set; }

    [JsonProperty("action")]
    public required string Action { get; set; }

    [JsonProperty("gmtCreate")]
    public required DateTimeOffset GmtCreate { get; set; }
}

public class ClosedBy
{
    [JsonProperty("memberId")]
    public required string MemberId { get; set; }

    [JsonProperty("alias")]
    public required string Alias { get; set; }

    [JsonProperty("role")]
    public required Role Role { get; set; }

    [JsonProperty("profile")]
    public required string Profile { get; set; }

    [JsonProperty("avatar")]
    public required string Avatar { get; set; }

    [JsonProperty("createdBy")]
    public required string CreatedBy { get; set; }

    [JsonProperty("gmtCreate")]
    public required DateTimeOffset GmtCreate { get; set; }

    [JsonProperty("gmtModified")]
    public required DateTimeOffset GmtModified { get; set; }
}

[JsonConverter(typeof(RoleConverter))]
public enum Role
{
    Admin,
    Member
};

[JsonConverter(typeof(StatusConverter))]
public enum Status
{
    Accepted,
    Cancelled,
    Closed
};

#region JsonConverter
internal class RoleConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(Role) || t == typeof(Role?);

    public override object? ReadJson(
        JsonReader reader,
        Type t,
        object? existingValue,
        JsonSerializer serializer
    )
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        var value = serializer.Deserialize<string>(reader);
        return value switch
        {
            "admin" => Role.Admin,
            "member" => Role.Member,
            _ => throw new Exception("Cannot unmarshal type Role")
        };
    }

    public override void WriteJson(
        JsonWriter writer,
        object? untypedValue,
        JsonSerializer serializer
    )
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (Role)untypedValue;
        switch (value)
        {
            case Role.Admin:
                serializer.Serialize(writer, "admin");
                return;
            case Role.Member:
                serializer.Serialize(writer, "member");
                return;
        }
        throw new Exception("Cannot marshal type Role");
    }
}

internal class StatusConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(Status) || t == typeof(Status?);

    public override object? ReadJson(
        JsonReader reader,
        Type t,
        object? existingValue,
        JsonSerializer serializer
    )
    {
        if (reader.TokenType == JsonToken.Null)
            return null;
        var value = serializer.Deserialize<string>(reader);
        return value switch
        {
            "accepted" => Status.Accepted,
            "cancelled" => Status.Cancelled,
            "closed" => Status.Closed,
            _ => throw new Exception("Cannot unmarshal type Status")
        };
    }

    public override void WriteJson(
        JsonWriter writer,
        object? untypedValue,
        JsonSerializer serializer
    )
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (Status)untypedValue;
        switch (value)
        {
            case Status.Accepted:
                serializer.Serialize(writer, "accepted");
                return;
            case Status.Cancelled:
                serializer.Serialize(writer, "cancelled");
                return;
            case Status.Closed:
                serializer.Serialize(writer, "closed");
                return;
        }
        throw new Exception("Cannot marshal type Status");
    }
}
#endregion
