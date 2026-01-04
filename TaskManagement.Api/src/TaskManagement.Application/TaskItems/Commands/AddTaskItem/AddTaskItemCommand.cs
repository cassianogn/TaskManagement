using TaskManagement.Domain.TaskItems;

namespace TaskManagement.Application.TaskItems.Commands.AddTaskItem
{
    public class AddTaskItemCommand
    {
        public AddTaskItemCommand() { }
        public AddTaskItemCommand(string title)
        {
            Title = title;
        }
        public string Title { get; set; }

        public TaskItem ToDomain()
        {
            return new TaskItem(Title, DateTime.UtcNow);
        }
    }
}
