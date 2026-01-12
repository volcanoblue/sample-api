using System.Net;
using System.Net.Http.Json;
using VolcanoBlue.SampleApi.Tests.Api.Fixture;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class GetUserByIdEndpointTests(ApiTestFixture fixture) : IClassFixture<ApiTestFixture>
    {
        

        [Fact]
        public async Task Should_return_200_when_query_has_a_result()
        {
            //Arrange
            var userCreated = await fixture.Client.PostAsJsonAsync(Endpoints.CreateUser, new { name = "John", email = "john@email.com" });
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>();
            
            //Act
            var userViewResponse = await fixture.Client.GetAsync($"/users/{user!.Id}");

            //Assert
            Assert.Equal(HttpStatusCode.OK, userViewResponse.StatusCode);
            Assert.Equal(user!.Id, (await fixture.Factory
                                                 .UserViewStore
                                                 .GetByIdAsync(user.Id, CancellationToken.None))
                                                 .ResultValue.Id);
        }

        [Fact]
        public async Task Should_return_304_when_etag_matches()
        {
            //Arrange
            var client = fixture.Client;
            var ct = CancellationToken.None;
            var userCreated = await client.PostAsJsonAsync(Endpoints.CreateUser, new { name = "John", email = "john@email.com" }, ct);
            var user = await userCreated.Content.ReadFromJsonAsync<UserCreatedResponse>(ct);
            
            //Act
            var userViewResponse = await client.GetAsync($"{Endpoints.GetUserById}/{user!.Id}", ct);
            var etag = userViewResponse.Headers.ETag!.Tag;

            client.DefaultRequestHeaders.IfNoneMatch.ParseAdd(etag);
            userViewResponse = await client.GetAsync($"{Endpoints.GetUserById}/{user!.Id}", ct);
            
            //Assert
            Assert.Equal(HttpStatusCode.NotModified, userViewResponse.StatusCode);
        }

        [Fact]
        public async Task Should_return_404_when_user_was_not_found()
        {
            //Act
            var userViewResponse = await fixture.Client.GetAsync($"{Endpoints.GetUserById}/{Guid.Empty}", CancellationToken.None);

            //Assert
            Assert.Equal(HttpStatusCode.NotFound, userViewResponse.StatusCode);
        }
    }
}
