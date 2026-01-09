using VolcanoBlue.SampleApi.Modules.Users.ChangeEmail;
using VolcanoBlue.SampleApi.Modules.Users.Domain;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class ChangeEmailHandlerTest
    {

        [Fact]
        public async Task Should_change_email_when_command_is_valid()
        {
            //Arrange
            var repo = new FakeUserRepository();
            var store = new FakeUserViewStore();
            var userId = Guid.NewGuid();
            await repo.SaveAsync(User.Create(userId, "John", "john@email.com"), CancellationToken.None);
            var handler = new ChangeEmailHandler(repo, store);
            var command = new ChangeEmailCommand(userId, "john@email2.com");
            var ct = CancellationToken.None;

            //Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            //Assert
            Assert.True(result);
            var user = repo.GetByIdAsync(userId, ct).ResultValue.Get();
            Assert.Equal("john@email2.com", user.Email);
        }

        [Fact]
        public async Task Should_fail_user_is_not_found()
        {
            //Arrange
            var handler = new ChangeEmailHandler(new FakeUserRepository(), new FakeUserViewStore());
            var command = new ChangeEmailCommand(Guid.NewGuid(), "john@email.com");

            //Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            //Assert
            Assert.False(result);
            Assert.IsType<UserNotFoundError>(result.ErrorValue);
        }

        [Fact]
        public async Task Should_fail_when_email_is_missing()
        {
            //Arrange
            var userId = Guid.NewGuid();
            var repo = new FakeUserRepository();
            await repo.SaveAsync(User.Create(userId, "John", "john@email.com"), CancellationToken.None);
            var handler = new ChangeEmailHandler(repo, new FakeUserViewStore());
            var command = new ChangeEmailCommand(userId, string.Empty);

            //Act
            var result = await handler.HandleAsync(command, CancellationToken.None);

            //Assert
            Assert.False(result);
            Assert.IsType<EmptyEmailError>(result.ErrorValue);
        }
    }
}
