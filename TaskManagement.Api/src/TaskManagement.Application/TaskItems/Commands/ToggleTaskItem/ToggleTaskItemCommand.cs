using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagement.Application.TaskItems.Commands.ToggleTaskItem
{
    public class ToggleTaskItemCommand
    {
        public Guid Id { get; set; }
        public ToggleTaskItemCommand(Guid id)
        {
            Id = id;
        }
    }
}
