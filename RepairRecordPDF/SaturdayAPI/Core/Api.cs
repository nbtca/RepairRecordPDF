using Newtonsoft.Json;
using SaturdayAPI.Core.Types;
using EventInfo = SaturdayAPI.Core.Types.EventInfo;

namespace SaturdayAPI.Core;

/// <summary>
/// 后端API对接
/// </summary>
public class Api
{
    public Api(string baseAddress = "https://api.nbtca.space/v2/")
    {
        HttpBaseAddress = baseAddress;
        HttpClient = new() { BaseAddress = new Uri(baseAddress) };
    }

    internal string HttpBaseAddress;
    internal HttpClient HttpClient;

    public async Task<EventInfo[]> GetEvents()
    {
        var response = await HttpClient.GetAsync("events");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<EventInfo[]>(content)
            ?? throw new NullReferenceException(nameof(content));
    }

    public async Task<EventInfo> GetEventById(int eventId)
    {
        var response = await HttpClient.GetAsync($"events/{eventId}");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<EventInfo>(content)
            ?? throw new NullReferenceException(nameof(content));
    }

    public async Task<MemberInfo[]> GetMembers()
    {
        var response = await HttpClient.GetAsync("members");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<MemberInfo[]>(content)
            ?? throw new NullReferenceException(nameof(content));
    }
}
