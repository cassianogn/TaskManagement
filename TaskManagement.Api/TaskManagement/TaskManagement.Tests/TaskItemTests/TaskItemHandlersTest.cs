
using Moq;
using TaskManagement.Application.Base.Handler;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Application.TaskItems.Queries;
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
            var addCommand = new AddTaskItemCommand("Test Task");
            var addHandler = new AddTaskItemCommandHandler(_taskItemRepositoryMock.Object);

            // Act
            var result = await addHandler.HandleAsync(addCommand);
            // Assert
            Assert.NotNull(result);

            // arrange 
            var command = new GetTaskItemsQuery(addCommand.Title);
            var handler = new GetTaskItemsQueryHandler(_taskItemRepositoryMock.Object);

            // Act
            HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>> queryResult = await handler.HandleAsync(command);
            // Assert
            Assert.NotNull(queryResult);
            Assert.NotNull(queryResult.Response);
            Assert.True(queryResult.IsSuccessful);
            Assert.Contains(queryResult.Response, item => item.Title.Contains(addCommand.Title));
        }
    }
}