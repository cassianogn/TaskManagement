namespace TaskManagement.Domain.TaskItems
{
    public interface ITaskItemRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<TaskItem> CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task<TaskItem> UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
