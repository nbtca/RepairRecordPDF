using Newtonsoft.Json;

namespace SaturdayAPI.Core.Types;

[JsonConverter(typeof(RoleConverter))]
public enum Role
{
    Admin,
    Member
};

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
