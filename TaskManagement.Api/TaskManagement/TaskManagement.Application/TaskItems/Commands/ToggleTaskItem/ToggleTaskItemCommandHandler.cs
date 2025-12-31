using TaskManagement.Application.Base.Handler;
using TaskManagement.Domain.Base; 
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application.TaskItems.Commands.ToggleTaskItem
{
    public class ToggleTaskItemCommandHandler : BaseHandler<ToggleTaskItemCommand, HandlerResponse>
    {
        public const string TaskNotFoundMessage = "Task not found.";
        private readonly ITaskItemRepository _repository;

        public ToggleTaskItemCommandHandler(ITaskItemRepository repository)
        {
            _repository = repository;
        }

        protected override async Task<HandlerResponse> BaseHandleAsync(ToggleTaskItemCommand command)
        {
            ValidationResult validation = command.Validate();
            if (!validation.IsValid) 
            {
                return HandlerResponse.DomainFailure(validation.Errors);
            }

            var taskItem = await _repository.GetByIdAsync(command.Id);
            if (taskItem is null)
            {
                return HandlerResponse.DomainFailure(TaskNotFoundMessage);
            }

            taskItem.ToggleStatus();
            await _repository.UpdateAsync(taskItem);
            return HandlerResponse.Success();
        }
    }
}