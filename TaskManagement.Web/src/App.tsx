import './App.css'
import { TaskForm } from './components/TaskForm';
import { TaskItem } from './components/TaskItem';
import { useTasks, useToggleTask } from './hooks/useTasks';

function App() {
  const { data: tasks, isLoading, error } = useTasks();
  const toggleTask = useToggleTask();

  const handleToggle = (id: string) => {
    toggleTask.mutate(id);
  };
  return (
    <div className="min-h-screen bg-gray-50 py-12 px-4">
      <div className="max-w-2xl mx-auto">
        <h1 className="text-4xl font-bold text-gray-900 mb-8 text-center">
          Todo List
        </h1>
        <div className="bg-white rounded-lg shadow-sm border border-gray-200 p-6">
          <TaskForm />

          {!isLoading && !error && tasks && tasks.length > 0 && (
            <div className="divide-y divide-gray-200">
              {tasks.map((task) => (
                <TaskItem key={task.id} task={task} onToggle={handleToggle} />
              ))}
            </div>
          )}
        </div>
      </div>
    </div>
  )
}

export default App
