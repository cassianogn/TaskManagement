using TaskManagement.Application.Base.Handler;

namespace TaskManagement.Tests.BaseHandlerTests.FakeEnvironment
{
    internal class FakeCommandHandlerWithResponse : BaseHandler<FakeCommand, HandlerResponse<Guid>>
    {
        protected override Task<HandlerResponse<Guid>> BaseHandleAsync(FakeCommand command)
        {
            return Task.FromResult(HandlerResponse<Guid>.Success(command.FakeId!.Value));
        }
    }

    internal class FakeCommandHandlerWithDomainError : BaseHandler<FakeCommand, HandlerResponse>
    {
        internal const string DomainErrorMessage = "A domain error occurred.";
        protected override Task<HandlerResponse> BaseHandleAsync(FakeCommand command)
        {
            return Task.FromResult(HandlerResponse.DomainFailure(DomainErrorMessage));
        }
    }

}
