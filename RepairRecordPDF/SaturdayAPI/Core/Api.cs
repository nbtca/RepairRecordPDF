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

    public async Task<IEnumerable<Events>> GetEvents()
    {
        var response = await HttpClient.GetAsync("events");
        response.EnsureSuccessStatusCode();
        var content = await response.Content.ReadAsStringAsync();
        return JsonConvert.DeserializeObject<Events[]>(content)
            ?? throw new NullReferenceException(nameof(content));
    }
}
