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
    public required MemberInfo Member { get; set; }

    [JsonProperty("closedBy")]
    public required MemberInfo ClosedBy { get; set; }

    [JsonProperty("status")]
    public required Status Status { get; set; }

    [JsonProperty("logs")]
    public EventLog[]? Logs { get; set; }

    [JsonProperty("gmtCreate")]
    public required DateTimeOffset GmtCreate { get; set; }

    [JsonProperty("gmtModified")]
    public required DateTimeOffset GmtModified { get; set; }
}
