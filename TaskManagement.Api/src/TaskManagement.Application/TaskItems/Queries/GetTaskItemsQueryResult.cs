using TaskManagement.Application.Base.Handler;
using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application.TaskItems.Queries
{
    public class GetTaskItemsQueryResult
    {
        public GetTaskItemsQueryResult(TaskItem taskItem)
        {
            Id = taskItem.Id;
            Title = taskItem.Title;
            IsCompleted = taskItem.IsCompleted;
        }

        public Guid Id { get; set; }
        public string Title { get; set; }
        public bool IsCompleted { get; set; }
    }
}
