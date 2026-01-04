import { type TaskItem as TaskItemType } from '../services/taskService';
import { Toggle } from './Toggle';

interface TaskItemProps {
  task: TaskItemType;
  onToggle: (id: string) => void;

}

export const TaskItem = ({ task, onToggle }: TaskItemProps) => {

  return (
    <div>
      <span>
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

