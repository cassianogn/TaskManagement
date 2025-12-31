using TaskManagement.Application.Base.Handler;
using TaskManagement.Domain.Base;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application.TaskItems.Commands.AddTaskItem
{
    public class AddTaskItemCommandHandler : BaseHandler<AddTaskItemCommand, HandlerResponse<Guid>>
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public AddTaskItemCommandHandler(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        protected override async Task<HandlerResponse<Guid>> BaseHandleAsync(AddTaskItemCommand command, CancellationToken cancellationToken)
        {
            ValidationResult commandValidation = command.Validate();
            if (!commandValidation.IsValid) return HandlerResponse<Guid>.DomainFailure(commandValidation.Errors.ToList());
            
            TaskItem task = command.ToDomain();
            ValidationResult duplicationValidation = await TaskItemDomainValidation.IsDuplicatedAsync(task, _taskItemRepository);
            if (!duplicationValidation.IsValid)
            {
                return HandlerResponse<Guid>.DomainFailure(duplicationValidation.Errors.ToList());
            }

            await _taskItemRepository.CreateAsync(task, cancellationToken);
            return HandlerResponse<Guid>.Success(task.Id);
        }
    }
}
