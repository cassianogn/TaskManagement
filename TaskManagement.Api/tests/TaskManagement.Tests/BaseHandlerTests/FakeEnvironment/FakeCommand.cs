namespace TaskManagement.Tests.BaseHandlerTests.FakeEnvironment
{
    internal class FakeCommand
    {
        public FakeCommand(){}
        public FakeCommand(Guid? fakeId)
        {
            FakeId = fakeId;
        }

        public Guid? FakeId { get; set; }
    }
}