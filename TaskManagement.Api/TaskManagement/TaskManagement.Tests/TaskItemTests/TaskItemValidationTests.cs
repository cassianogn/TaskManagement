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
/*
 * namespace TaskManagement.Domain.TaskItems
{
    public class TaskItem
    {
        private TaskItem()
        {
        }
        public TaskItem(string title, DateTime createdAt)
        {
            Id = Guid.NewGuid();
            Title = title;
            CreatedAt = createdAt;

            TaskItemDomainValidation.ThrowIfInvalid(this);
        }
        public Guid Id { get; }
        public string Title { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; }
        public DateTime? UpdatedAt { get; private set; }

        public void ToggleStatus()
        {
            IsCompleted = !IsCompleted;
            UpdatedAt = DateTime.UtcNow;
         
            TaskItemDomainValidation.ThrowIfInvalid(this);
        }
        public void Update(string title, string? description = null)
        {
            Title = title;
            UpdatedAt = DateTime.UtcNow;
         
            TaskItemDomainValidation.ThrowIfInvalid(this);
        }
    }
}

 */
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TaskManagement.Domain.Base;

//namespace TaskManagement.Domain.TaskItems
//{
//    public static class TaskItemDomainValidation
//    {
//public const string IdMustBeValidGuid = "Id must be a valid GUID";
//public const string TitleIsRequired = "Title is required";
//public const string TitleMustBeLessThan200Characters = "Title must be less than 200 characters";
//public const string TitleMustBeAtLeast3Characters = "Title must be at least 3 characters";
//public const string CreatedAtMustBeValidDate = "CreatedAt must be a valid date";
//public const string CreatedAtCannotBeGreaterThanNow = "CreatedAt cannot be in the future";
//public const string UpdatedAtMustBeSetWhenCompleted = "UpdatedAt must be set when task is completed";

//        public static async Task<ValidationResult> IsDuplicatedAsync(TaskItem newTaskItem, ITaskItemRepository repository)
//        {
//            IEnumerable<TaskItem> existentTasks = await repository.SearchAsync(newTaskItem.Title);
//            var isDuplicated = existentTasks.Any(existentTask => existentTask.Title == newTaskItem.Title);

//            if (isDuplicated)
//            {
//                return ValidationResult.Failure("A task with the same title already exists.");
//            }

//            return ValidationResult.Success();
//        }
//        public static void ThrowIfInvalid(TaskItem taskItem)
//        {
//            var errors = new List<string>(4);

//            if (taskItem.Id == Guid.Empty)
//            {
//                errors.Add("Id must be a valid GUID");
//            }

//            if (string.IsNullOrWhiteSpace(taskItem.Title))
//            {
//                errors.Add("Title is required");
//            }
//            else if (taskItem.Title.Length > 200)
//            {
//                errors.Add("Title must be less than 200 characters");
//            }
//            else if (taskItem.Title.Length < 3)
//            {
//                errors.Add("Title must be at least 3 characters");
//            }

//            if (taskItem.CreatedAt == default)
//            {
//                errors.Add("CreatedAt must be a valid date");
//            }
//            else if (taskItem.CreatedAt > DateTime.UtcNow)
//            {
//                errors.Add("CreatedAt cannot be in the future");
//            }

//            if (taskItem.IsCompleted && taskItem.UpdatedAt == null)
//            {
//                errors.Add("UpdatedAt must be set when task is completed");
//            }

//            if (errors.Count > 0)
//            {
//                throw new DomainValidationException(string.Join("\n", errors));
//            }
//        }

//    }
//}
