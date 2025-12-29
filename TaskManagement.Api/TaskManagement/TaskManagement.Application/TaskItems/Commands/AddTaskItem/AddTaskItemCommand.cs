using TaskManagement.Domain.Base;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application.TaskItems.Commands.AddTaskItem
{
    public class AddTaskItemCommand
    {
        public AddTaskItemCommand() { }
        public AddTaskItemCommand(string title)
        {
            Title = title;
        }
        public string Title { get; set; }

        public TaskItem ToDomain()
        {
            return new TaskItem(Title, DateTime.UtcNow);
        }
    }
    public static class AddTaskItemValidationCommand
    {
        public static ValidationResult IsValid(this AddTaskItemCommand Command)
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
