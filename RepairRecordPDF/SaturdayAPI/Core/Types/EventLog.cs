using Newtonsoft.Json;

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
    public required string Action { get; set; }

    [JsonProperty("gmtCreate")]
    public required DateTimeOffset GmtCreate { get; set; }
}