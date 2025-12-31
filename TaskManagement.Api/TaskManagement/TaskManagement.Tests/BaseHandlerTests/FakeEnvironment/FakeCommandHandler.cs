using TaskManagement.Application.Base.Handler;

namespace TaskManagement.Tests.BaseHandlerTests.FakeEnvironment
{
    internal class FakeCommandHandler : BaseHandler<FakeCommand, HandlerResponse>
    {
        protected override Task<HandlerResponse> BaseHandleAsync(FakeCommand command, CancellationToken cancellationToken)
        {
            return Task.FromResult(HandlerResponse.Success());
        }
    }
}
