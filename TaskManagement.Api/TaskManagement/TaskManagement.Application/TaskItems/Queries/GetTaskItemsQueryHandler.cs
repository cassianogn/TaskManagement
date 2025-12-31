using TaskManagement.Application.Base.Handler;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application.TaskItems.Queries
{
    public class GetTaskItemsQueryHandler : BaseHandler<GetTaskItemsQuery, HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>>>
    {
        private readonly ITaskItemRepository _taskItemRepository;

        public GetTaskItemsQueryHandler(ITaskItemRepository taskItemRepository)
        {
            _taskItemRepository = taskItemRepository;
        }

        protected override async Task<HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>>> BaseHandleAsync(GetTaskItemsQuery command, CancellationToken cancellationToken)
        {
            IReadOnlyCollection<TaskItem> taskItems = await _taskItemRepository.SearchAsync(command.SearchKey);
            IReadOnlyCollection<GetTaskItemsQueryResult> result = taskItems.Select(taskItem => new GetTaskItemsQueryResult(taskItem))
                                                                           .ToList();
            return HandlerResponse<IReadOnlyCollection<GetTaskItemsQueryResult>>.Success(result);
        }
    }
}
