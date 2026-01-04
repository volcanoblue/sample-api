namespace VolcanoBlue.SampleApi.Tests.Api.Fixture
{
    public sealed class ApiTestFixture
    {
        public CustomWebApplicationFactory Factory { get; } = new();
        public HttpClient Client { get; }

        public ApiTestFixture()
        {
            Client = Factory.CreateClient();
        }
    }
}