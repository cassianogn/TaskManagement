import { useState, type FormEvent } from 'react';
import { Plus } from 'lucide-react';
import { useCreateTask } from '../hooks/useTasks';

export const TaskForm = () => {
  const [title, setTitle] = useState('');
  const createTask = useCreateTask();

  const handleSubmit = async (e: FormEvent) => {
    e.preventDefault();
    
    if (title.trim().length < 3) {
      return;
    }

    try {
      await createTask.mutateAsync({ title: title.trim() });
      setTitle('');
    } catch (error) {
      console.error('Failed to create task:', error);
    }
  };

  const isDisabled = title.trim().length < 3 || createTask.isPending;

  return (
    <form onSubmit={handleSubmit}>
      <input
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Add a new task..."
        disabled={createTask.isPending}
      />
      <button
        type="submit"
        disabled={isDisabled}>
        <Plus size={20} />
        Add
      </button>
    </form>
  );
};

