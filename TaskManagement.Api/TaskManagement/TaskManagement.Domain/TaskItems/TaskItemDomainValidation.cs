using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagement.Domain.Base;

namespace TaskManagement.Domain.TaskItems
{
    public static class TaskItemDomainValidation
    {
        public const string IdMustBeValidGuid = "Id must be a valid GUID";
        public const string TitleIsRequired = "Title is required";
        public const string TitleMustBeLessThan200Characters = "Title must be less than 200 characters";
        public const string TitleMustBeAtLeast3Characters = "Title must be at least 3 characters";
        public const string CreatedAtMustBeValidDate = "CreatedAt must be a valid date";
        public const string CreatedAtCannotBeGreaterThanNow = "CreatedAt cannot be in the future";
        public const string UpdatedAtMustBeSetWhenCompleted = "UpdatedAt must be set when task is completed";

        public static void ThrowIfInvalid(TaskItem taskItem)
        {
            var errors = new List<string>(4);

            if (taskItem.Id == Guid.Empty)
            {
                errors.Add(IdMustBeValidGuid);
            }

            if (string.IsNullOrWhiteSpace(taskItem.Title))
            {
                errors.Add(TitleIsRequired);
            }
            else if (taskItem.Title.Length > 200)
            {
                errors.Add(TitleMustBeLessThan200Characters);
            }
            else if (taskItem.Title.Length < 3)
            {
                errors.Add(TitleMustBeAtLeast3Characters);
            }

            if (taskItem.CreatedAt == default)
            {
                errors.Add(CreatedAtMustBeValidDate);
            }
            else if (taskItem.CreatedAt > DateTime.UtcNow)
            {
                errors.Add(CreatedAtCannotBeGreaterThanNow);
            }

            if (taskItem.IsCompleted && taskItem.UpdatedAt == null)
            {
                errors.Add(UpdatedAtMustBeSetWhenCompleted);
            }

            if (errors.Count > 0)
            {
                throw new DomainValidationException(string.Join("\n", errors));
            }
        }

        public static async Task<ValidationResult> IsDuplicatedAsync(TaskItem taskItem, ITaskItemRepository repository)
        {
            IReadOnlyCollection<TaskItem> existentTasks = await repository.SearchAsync(taskItem.Title);
            var isDuplicated = existentTasks.Any(existentTask => existentTask.Title == taskItem.Title && existentTask.Id != taskItem.Id);

            if (isDuplicated)
            {
                return ValidationResult.Failure("A task with the same title already exists.");
            }

            return ValidationResult.Success();
        }

    }
}
