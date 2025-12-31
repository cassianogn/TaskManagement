using Microsoft.EntityFrameworkCore.ChangeTracking;
using Moq;
using TaskManagement.Application.Base.Handler;
using TaskManagement.Application.TaskItems.Commands.AddTaskItem;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Tests.TaskItemTests
{
    public class AddTaskItemCommandHandlerTests
    {
        private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new Mock<ITaskItemRepository>();

        [Fact]
        public async Task HandleAsync_InvalidTitle_ShouldReturnInvalidCommand()
        {
            // Arrange
            var command = new AddTaskItemCommand("AB");

            // Act
            var handler = new AddTaskItemCommandHandler(_taskItemRepositoryMock.Object);
            HandlerResponse<Guid> response = await handler.HandleAsync(command, default);

            // Assert
            BasicFailureAsserts(response);
            Assert.Contains(TaskItemDomainValidation.TitleMustBeAtLeast3Characters, response.ErrorMessage!);
        }

        [Fact]
        public async Task HandleAsync_DuplicateTitle_ShouldReturnDomainFailure()
        {
            // Arrange
            var command = new AddTaskItemCommand("Unique Title");
            _taskItemRepositoryMock
                .Setup(r => r.SearchAsync(command.Title, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskItem>() { new TaskItem(command.Title, DateTime.Now) });

            // Act
            var handler = new AddTaskItemCommandHandler(_taskItemRepositoryMock.Object);
            HandlerResponse<Guid> response = await handler.HandleAsync(command, default);

            // Assert
            BasicFailureAsserts(response);
            Assert.Contains(TaskItemDomainValidation.DuplicatedTitleError, response.ErrorMessage!);
        }

        private void BasicFailureAsserts(HandlerResponse<Guid> response)
        {
            Assert.False(response.IsSuccessful);
            Assert.NotNull(response.ErrorMessage);
            _taskItemRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact] 
        public async Task HandleAsync_ValidCommand_ShouldCreateTaskItemSuccessfully()
        {
            // Arrange
            var command = new AddTaskItemCommand("Valid Title");
            _taskItemRepositoryMock
                .Setup(r => r.SearchAsync(command.Title, It.IsAny<CancellationToken>()))
                .ReturnsAsync(new List<TaskItem>());
         
            // Act
            var handler = new AddTaskItemCommandHandler(_taskItemRepositoryMock.Object);
            HandlerResponse<Guid> response = await handler.HandleAsync(command, default);

            // Assert
            Assert.True(response.IsSuccessful);
            Assert.NotEqual(Guid.Empty, response.Response);
            _taskItemRepositoryMock.Verify(r => r.CreateAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
