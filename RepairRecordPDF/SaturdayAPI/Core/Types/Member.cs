using Newtonsoft.Json;

namespace SaturdayAPI.Core.Types;

public class MemberInfo
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

    public override string ToString() => $"{Alias} ({MemberId})";
}
