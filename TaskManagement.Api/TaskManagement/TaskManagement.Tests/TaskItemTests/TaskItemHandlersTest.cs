
using Moq;
using TaskManagement.Application.Base.Handler;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Tests.TaskItemTests
{
    public class TaskItemHandlersTest
    {
        private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new Mock<ITaskItemRepository>();
        [Fact]
        public async Task HandleTaskItem_CorrectlyProcessesValidTaskItemAsync()
        {
            // Arrange
            var command = new AddTaskItemCommand("Test Task");
            var handler = new AddTaskItemCommandHandler(_taskItemRepositoryMock.Object);

            // Act
            var result = await handler.HandleAsync(command);
            // Assert
            Assert.NotNull(result);
        }


    }
}