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
    <form onSubmit={handleSubmit} className="flex gap-2 mb-6">
      <input
        type="text"
        value={title}
        onChange={(e) => setTitle(e.target.value)}
        placeholder="Add a new task..."
        className="flex-1 px-4 py-2 border border-gray-300 rounded-lg focus:outline-none focus:ring-2 focus:ring-black focus:border-transparent"
        disabled={createTask.isPending}
      />
      <button
        type="submit"
        className="px-6 py-2 bg-black text-white rounded-lg hover:bg-gray-800 disabled:bg-gray-300 disabled:cursor-not-allowed transition-colors flex items-center gap-2"
        disabled={isDisabled}>
        <Plus size={20} />
        Add
      </button>
    </form>
  );
};

