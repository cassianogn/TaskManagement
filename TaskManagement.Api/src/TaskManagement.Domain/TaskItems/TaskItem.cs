namespace TaskManagement.Domain.TaskItems
{
    public class TaskItem
    {
        private TaskItem()
        {
        }
        public TaskItem(string title, DateTime createdAt)
        {
            Id = Guid.NewGuid();
            Title = title;
            CreatedAt = createdAt;

            TaskItemDomainValidation.ThrowIfInvalid(this);
        }
        public Guid Id { get; private set; }
        public string Title { get; private set; }
        public bool IsCompleted { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }

        public void ToggleStatus()
        {
            IsCompleted = !IsCompleted;
            UpdatedAt = DateTime.UtcNow;
         
            TaskItemDomainValidation.ThrowIfInvalid(this);
        }
        public void Update(string title, string? description = null)
        {
            Title = title;
            UpdatedAt = DateTime.UtcNow;
         
            TaskItemDomainValidation.ThrowIfInvalid(this);
        }

        
    }
}
