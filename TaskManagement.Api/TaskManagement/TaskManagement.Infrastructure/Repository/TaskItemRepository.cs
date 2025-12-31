using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.TaskItems;
using TaskManagement.Infrastructure.Data;

namespace TaskManagement.Infrastructure.Repository
{
    internal class TaskItemRepository : ITaskItemRepository
    {
        private readonly TaskManagementDbContext _context;

        public TaskItemRepository(TaskManagementDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            await _context.TaskItems.AddAsync(taskItem, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UpdateAsync(TaskItem taskItem, CancellationToken cancellationToken = default)
        {
            _context.TaskItems.Update(taskItem);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<bool> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            var task = await _context.TaskItems.FindAsync(new object[] { id }, cancellationToken);
            if (task == null) return false;

            _context.TaskItems.Remove(task);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }

        public async Task<IReadOnlyCollection<TaskItem>> SearchAsync(string? key, CancellationToken cancellationToken = default)
        {
            var query = _context.TaskItems.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(key))
            {
                query = query.Where(t => t.Title.Contains(key));
            }

            var result = await query.ToListAsync(cancellationToken);
            return result.AsReadOnly();
        }
    }
}
