using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Tests.TaskItemTests
{
    public class TaskItemValidationTests
    {
        private readonly Mock<ITaskItemRepository> _taskItemRepositoryMock = new Mock<ITaskItemRepository>();
        
        [Fact]
        public void TaskItemValidation_ShouldThrowException_WhenTitleIsEmpty()
        {
            // Arrange & Act & Assert
            DomainValidationException exception = Assert.Throws<DomainValidationException>(() => new TaskItem("", DateTime.UtcNow));
            Assert.Equal(TaskItemDomainValidation.TitleIsRequired, exception.Message);
        }

        [Fact]
        public void TaskItemValidation_ShouldThrowException_WhenTitleIsTooLong()
        {
            // Arrange 
            var longTitle = new string('A', 201);

            // Act & Assert
            DomainValidationException exception = Assert.Throws<DomainValidationException>(() => new TaskItem(longTitle, DateTime.UtcNow));
            Assert.Equal(TaskItemDomainValidation.TitleMustBeLessThan200Characters, exception.Message);
        }

        [Fact]
        public void TaskItemValidation_ShouldThrowException_WhenTitleIsTooShort()
        {
            var shortTitle = "AB";
            
            // Act & Assert
            DomainValidationException exception = Assert.Throws<DomainValidationException>(() => new TaskItem(shortTitle, DateTime.UtcNow));
            Assert.Equal(TaskItemDomainValidation.TitleMustBeAtLeast3Characters, exception.Message);
        }

        [Fact]
        public void TaskItemValidation_ShouldThrowException_WhenCreatedAtIsInFuture()
        {
            // Arrange
            DateTime futureDate = DateTime.UtcNow.AddMinutes(10);
            
            // Act & Assert
            DomainValidationException exception = Assert.Throws<DomainValidationException>(() => new TaskItem("Valid Title", futureDate));
            Assert.Equal(TaskItemDomainValidation.CreatedAtCannotBeGreaterThanNow, exception.Message);
        }

        [Fact]
        public void TaskItemValidation_ShouldThrowException_WhenIdIsInvalid()
        {
            // Arrange
            var taskItem = new TaskItem("Valid Title", DateTime.UtcNow);
            PropertyInfo invalidTaskItemIdProperty = typeof(TaskItem).GetProperty("Id")!;
            invalidTaskItemIdProperty.SetValue(taskItem, Guid.Empty);
            
            // Act & Assert
            DomainValidationException exception = Assert.Throws<DomainValidationException>(() => TaskItemDomainValidation.ThrowIfInvalid(taskItem));
            Assert.Equal(TaskItemDomainValidation.IdMustBeValidGuid, exception.Message);
        }

        [Fact]
        public void TaskItemValidation_ShouldThrowException_WhenUpdatedAtIsNullOnCompletion()
        {
            // Arrange
            var taskItem = new TaskItem("Valid Title", DateTime.UtcNow);
            taskItem.ToggleStatus();
            PropertyInfo updatedAtProperty = typeof(TaskItem).GetProperty("UpdatedAt")!;
            updatedAtProperty.SetValue(taskItem, null);

            // Act & Assert
            DomainValidationException exception = Assert.Throws<DomainValidationException>(() => TaskItemDomainValidation.ThrowIfInvalid(taskItem));
            Assert.Equal(TaskItemDomainValidation.UpdatedAtMustBeSetWhenCompleted, exception.Message);
        }

        [Fact]
        public async Task TaskItemValidation_ShouldValidateDuplicateTitleAsync_WhenTitleIsDuplicated()
        {
            // Arrange
            var existingTaskItem = new TaskItem("Duplicate Title", DateTime.UtcNow);
            
            ITaskItemRepository repository = _taskItemRepositoryMock.Object;
            _taskItemRepositoryMock.Setup(r => r.SearchAsync("Duplicate Title", default))
                .ReturnsAsync(new List<TaskItem> { existingTaskItem });
            var newTaskItem = new TaskItem("Duplicate Title", DateTime.UtcNow);

            // Act
            ValidationResult validationResultTask = await TaskItemDomainValidation.IsDuplicatedAsync(newTaskItem, repository);

            // Assert
            Assert.False(validationResultTask.IsValid);
            Assert.Contains("A task with the same title already exists.", validationResultTask.Errors);
        }

        [Fact]
        public async Task TaskItemValidation_ShouldPassDuplicateTitleCheckAsync_WhenTitleIsUnique()
        {
            // Arrange
            ITaskItemRepository repository = _taskItemRepositoryMock.Object;
            var newTaskItem = new TaskItem("Unique Title", DateTime.UtcNow);
            
            // Act
            ValidationResult validationResultTask = await TaskItemDomainValidation.IsDuplicatedAsync(newTaskItem, repository);
            
            // Assert
            Assert.True(validationResultTask.IsValid);
            Assert.Empty(validationResultTask.Errors);
        }

        [Fact]
        public async Task TaskItemValidation_ShouldPassDuplicateTitleCheckAsync_WhenEntityAlreadyExists()
        {
            // Arrange
            var existingTaskItem = new TaskItem("Existing Title", DateTime.UtcNow);
            
            ITaskItemRepository repository = _taskItemRepositoryMock.Object;
            _taskItemRepositoryMock.Setup(r => r.SearchAsync("Existing Title", default))
                .ReturnsAsync(new List<TaskItem> { existingTaskItem });
            var updatedTaskItem = existingTaskItem;
            // Act
            ValidationResult validationResultTask = await TaskItemDomainValidation.IsDuplicatedAsync(updatedTaskItem, repository);
            // Assert
            Assert.True(validationResultTask.IsValid);
            Assert.Empty(validationResultTask.Errors);
        }

    }
}