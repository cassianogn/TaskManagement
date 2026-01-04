using TaskManagement.Domain.Base;

namespace TaskManagement.Application.TaskItems.Commands.AddTaskItem
{
    public static class AddTaskItemCommandValidation
    {
        public static ValidationResult Validate(this AddTaskItemCommand Command)
        {
            string? error = null;
            if (string.IsNullOrWhiteSpace(Command.Title))
            {
                error = "Title is required";
            }
            else if (Command.Title.Length > 200)
            {
                error = "Title must be less than 200 characters";
            }
            else if (Command.Title.Length < 3)
            {
                error = "Title must be at least 3 characters";
            }
            return error is null ? ValidationResult.Success() : ValidationResult.Failure(new List<string> { error });
        }
    }
}
