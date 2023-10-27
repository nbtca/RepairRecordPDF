using Newtonsoft.Json;
using SaturdayAPI.Core.Types;

namespace SaturdayAPI.Core;

public class Api
{
    public Api(string address = "https://api.nbtca.space/v2/")
    {
        HttpAddress = address;
        HttpClient = new() { BaseAddress = new Uri(address) };
    }

    internal string HttpAddress;
    internal HttpClient HttpClient;

    public async Task<IEnumerable<EventInfo>> GetEvents()
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
}
