using Moq;
using TaskManagement.Application.TaskItems.Commands.ToggleTaskItem;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Tests.TaskItemTests
{
    public class ToggleTaskItemCommandHandlerTests
    {
        private readonly Mock<ITaskItemRepository> _repositoryMock = new();

        [Fact]
        public async Task Handle_ShouldReturnError_WhenTaskDoesNotExist()
        {
            // Arrange
            var command = new ToggleTaskItemCommand(Guid.NewGuid());
            _repositoryMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                           .ReturnsAsync((TaskItem?)null);

            var handler = new ToggleTaskItemCommandHandler(_repositoryMock.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.False(result.IsSuccessful);
            Assert.Equal(ToggleTaskItemCommandHandler.TaskNotFoundMessage, result.ErrorMessage!.First());
            _repositoryMock.Verify(x => x.UpdateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldToggleAndPersist_WhenTaskExists()
        {
            // Arrange
            var task = new TaskItem("Test Task", DateTime.UtcNow); 
            var command = new ToggleTaskItemCommand(task.Id);

            _repositoryMock.Setup(x => x.GetByIdAsync(task.Id, It.IsAny<CancellationToken>()))
                           .ReturnsAsync(task);

            var handler = new ToggleTaskItemCommandHandler(_repositoryMock.Object);

            // Act
            var result = await handler.HandleAsync(command);

            // Assert
            Assert.True(result.IsSuccessful);
            Assert.True(task.IsCompleted); 
            _repositoryMock.Verify(x => x.UpdateAsync(task, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
