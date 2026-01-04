import { type TaskItem as TaskItemType } from '../services/taskService';
import { Toggle } from './Toggle';

interface TaskItemProps {
  task: TaskItemType;
  onToggle: (id: string) => void;

}

export const TaskItem = ({ task, onToggle }: TaskItemProps) => {

  return (
    <div className="flex items-center justify-between p-4 border-b border-gray-200 hover:bg-gray-50 transition-colors">
      <span
        className={`flex-1 text-lg ${task.isCompleted ? 'line-through text-gray-400' : 'text-gray-900'
          }`}
      >
        {task.title}
      </span>
      <Toggle
        checked={task.isCompleted}
        onChange={() => onToggle(task.id)}
        aria-label={`Toggle task: ${task.title}`}
      />
    </div>
  );
};

