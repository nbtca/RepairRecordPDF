namespace SaturdayAPI.Test
{
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
    }
}
