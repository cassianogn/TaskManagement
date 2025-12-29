using TaskManagement.Application.Base.Handler;
using TaskManagement.Tests.BaseHandlerTests.FakeEnvironment;

namespace TaskManagement.Tests.BaseHandlerTests
{

    public class BaseHandlerTest
    {
        [Fact]
        public async Task BaseHandler_ShouldRun_SuccessAsync()
        {
            //Arrange
            var command = new FakeCommand();
            var handler = new FakeCommandHandler();

            // Act
            var result = await handler.HandleAsync(command);
            
            // Assert
            Assert.NotNull(handler);
            Assert.True(result.IsSuccessful);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);
        }
        [Fact]
        public async Task BaseHandler_ShouldRun_SuccessWithResponseAsync()
        {
            //Arrange
            var command = new FakeCommand(Guid.NewGuid());
            var handler = new FakeCommandHandlerWithResponse();

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.NotNull(handler);
            Assert.True(result.IsSuccessful);
            Assert.Equal(command.FakeId, result.Response);
            Assert.Null(result.ErrorMessage);
            Assert.Null(result.Exception);

        }
        [Fact]
        public async Task BaseHandler_ShouldRun_ThenThrowHandlerExceptionAsync()
        {
            //Arrange
            var command = new FakeCommand();
            var handler = new FakeCommandHandlerWithError();
            var baseHandlerErrorMessage = $"Unexpected error occurred while handling command of type {typeof(FakeCommand).FullName}. Command parameters: {{\"FakeId\":null}}";
            
            // Act 
            var exception = await Assert.ThrowsAsync<HandlerException>(() => handler.HandleAsync(command));

            // Assert
            Assert.NotNull(exception);
            Assert.Contains(FakeCommandHandlerWithError.ErrorMessage, exception.InnerException!.Message);
            Assert.Equal(baseHandlerErrorMessage, exception.Message);
        }

        [Fact]
        public async Task BaseHandler_ShouldRun_ThenReturnDomainErrorAsync()
        {
            //Arrange
            var command = new FakeCommand();
            var handler = new FakeCommandHandlerWithDomainError();
            // Act
            var result = await handler.HandleAsync(command);
            // Assert
            Assert.NotNull(handler);
            Assert.False(result.IsSuccessful);
            Assert.Equal(FakeCommandHandlerWithDomainError.DomainErrorMessage, result.ErrorMessage);
            Assert.Null(result.Exception);
        }
    }
}
