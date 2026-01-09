using VolcanoBlue.SampleApi.Abstractions;
using VolcanoBlue.SampleApi.Modules.Users.CreateUser;
using VolcanoBlue.SampleApi.Modules.Users.Shared;

namespace VolcanoBlue.SampleApi.Tests.Modules.Users
{
    public class CreateUserHandlerTests
    {
        [Fact]
        public async Task Should_create_user_when_command_is_valid()
        {
            var handler = new CreateUserHandler(new FakeUserRepository(), new FakeUserViewStore());

            var command = new CreateUserCommand("John", "john@email.com");

            var result = await handler.HandleAsync(command, CancellationToken.None);

            Assert.True(result);
        }

        [Fact]
        public async Task Should_fail_when_name_is_missing()
        {
            var handler = new CreateUserHandler(new FakeUserRepository(), new FakeUserViewStore());
            var command = new CreateUserCommand(string.Empty, "john@email.com");

            var result = await handler.HandleAsync(command, CancellationToken.None);

            Assert.False(result);
            Assert.IsType<EmptyNameError>(result.ErrorValue);
        }

        [Fact]
        public async Task Should_fail_when_email_is_missing()
        {
            var handler = new CreateUserHandler(new FakeUserRepository(), new FakeUserViewStore());
            var command = new CreateUserCommand("John", string.Empty);

            var result = await handler.HandleAsync(command, CancellationToken.None);

            Assert.False(result);
            Assert.IsType<EmptyEmailError>(result.ErrorValue);
        }

        [Fact]
        public async Task Should_fail_when_cancellation_is_requested() 
        {
            //Arrange
            var handler = new CreateUserHandler(new FakeUserRepository(), new FakeUserViewStore());
            var command = new CreateUserCommand("John", "john@email.com");
            var cts = new CancellationTokenSource();
            cts.Cancel();
            var ct = cts.Token;

            //Act
            var result = await handler.HandleAsync(command, ct);

            //Assert
            Assert.False(result);
            Assert.IsType<OperationCancelledError>(result.ErrorValue);
        }
    }
}
