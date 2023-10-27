using Newtonsoft.Json;

namespace SaturdayAPI.Test;

public class Tests
{
    Core.Api api;

    [SetUp]
    public void Setup()
    {
        api = new();
    }

    [Test]
    public async Task TestEvents()
    {
        var result = await api.GetEvents();
        foreach (var events in result)
        {
            Console.WriteLine(events.EventId);
        }
        Assert.Pass();
    }

    [Test]
    public async Task TestEventId()
    {
        var result = await api.GetEventById(2);
        Console.WriteLine(JsonConvert.SerializeObject(result));
        Assert.Pass();
    }
}
