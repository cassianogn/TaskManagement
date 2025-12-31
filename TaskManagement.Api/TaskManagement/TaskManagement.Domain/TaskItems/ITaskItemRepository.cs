namespace TaskManagement.Domain.TaskItems
{
    public interface ITaskItemRepository
    {
        Task<IReadOnlyCollection<TaskItem>> SearchAsync(string? key, CancellationToken cancellationToken = default);
        Task CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
