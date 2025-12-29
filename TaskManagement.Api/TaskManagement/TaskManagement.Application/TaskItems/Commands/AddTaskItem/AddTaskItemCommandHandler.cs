using TaskManagement.Application.Base.Handler;
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

        protected override async Task<HandlerResponse<Guid>> BaseHandleAsync(AddTaskItemCommand command)
        {
            var commandValidation = command.IsValid();
            if (!commandValidation.IsValid) return HandlerResponse<Guid>.DomainFailure(commandValidation.Errors.ToList());
            
            var task = command.ToDomain();

            var duplicationValidation = await TaskItemDomainValidation.IsDuplicatedAsync(task, _taskItemRepository);
            if (!duplicationValidation.IsValid)
            {
                return HandlerResponse<Guid>.DomainFailure(duplicationValidation.Errors.ToList());
            }

            await _taskItemRepository.CreateAsync(task);
            return HandlerResponse<Guid>.Success(task.Id);
        }
    }
}
