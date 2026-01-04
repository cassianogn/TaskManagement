import { type TaskItem as TaskItemType } from '../services/taskService';

interface TaskItemProps {
  task: TaskItemType;
}

export const TaskItem = ({ task }: TaskItemProps) => {
  return (
    <div>
      <span>
        {task.title}
      </span>
    </div>
  );
};

