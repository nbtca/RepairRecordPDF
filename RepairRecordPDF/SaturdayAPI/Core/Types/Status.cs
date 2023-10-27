using Newtonsoft.Json;

namespace SaturdayAPI.Core.Types;

[JsonConverter(typeof(StatusConverter))]
public enum Status
{
    Open,
    Accepted,
    Cancelled,
    Closed,
    Committed
};

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
            "open" => Status.Open,
            "accepted" => Status.Accepted,
            "cancelled" => Status.Cancelled,
            "closed" => Status.Closed,
            "committed" => Status.Committed,
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
            case Status.Open:
                serializer.Serialize(writer, "open");
                return;
            case Status.Committed:
                serializer.Serialize(writer, "committed");
                return;
        }
        throw new Exception("Cannot marshal type Status");
    }
}
