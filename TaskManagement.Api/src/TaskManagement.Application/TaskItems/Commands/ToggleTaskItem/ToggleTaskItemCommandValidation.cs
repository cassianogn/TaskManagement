using TaskManagement.Domain.Base;

namespace TaskManagement.Application.TaskItems.Commands.ToggleTaskItem
{
    public static class ToggleTaskItemCommandValidation
    {
        public const string IdIsRequired = "Id is required";
        public static ValidationResult Validate(this ToggleTaskItemCommand Command)
        {
            string? error = null;
            if (Command.Id == Guid.Empty)
            {
                error = IdIsRequired;
            }
            return error is null ? ValidationResult.Success() : ValidationResult.Failure(error);
        }
    }
}

