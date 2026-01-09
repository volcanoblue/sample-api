using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.GetUser;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class GetByIdHandlerTests
    {
        [Fact]
        public async Task Shound_return_an_user_view_when_user_exists()
        {
            //Arrange
            var store = new FakeUserViewStore();
            var userView = new UserView(Guid.Empty, "Name", "Email");
            await store.StoreAsync(userView, CancellationToken.None);

            //Act
            var handler = new GetUserByIdHandler(store);
            var result = await handler.HandleAsync(new GetUserByIdQuery(Guid.Empty), CancellationToken.None);

            //Assert
            Assert.True(result.IsOk);
            Assert.True(result.ResultValue.IsSome);
            Assert.Equal(userView, result.ResultValue.Get());
        }

        [Fact]
        public async Task Shound_return_an_error_when_user_dont_exists()
        {
            //Arrange
            var handler = new GetUserByIdHandler(new FakeUserViewStore());

            //Act
            var result = await handler.HandleAsync(new GetUserByIdQuery(Guid.Empty), CancellationToken.None);

            //Assert
            Assert.True(result.IsError);
            Assert.IsType<UserNotFoundError>(result.ErrorValue);
        }
    }
}
