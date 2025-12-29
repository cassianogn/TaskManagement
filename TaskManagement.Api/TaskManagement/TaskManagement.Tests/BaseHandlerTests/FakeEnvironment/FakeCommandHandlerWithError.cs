using TaskManagement.Application.Base.Handler;

namespace TaskManagement.Tests.BaseHandlerTests.FakeEnvironment
{
    internal class FakeCommandHandlerWithError : BaseHandler<FakeCommand, HandlerResponse>
    {
        internal const string ErrorMessage = "An error occurred while handling the command.";
        protected override Task<HandlerResponse> BaseHandleAsync(FakeCommand command)
        {
            throw new Exception(ErrorMessage);
        }
    }
}
