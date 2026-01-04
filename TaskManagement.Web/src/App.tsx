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
    <>
      <TaskForm />

      {!isLoading && !error && tasks && tasks.length > 0 && (
        <div className="divide-y divide-red-500">
          {tasks.map((task) => (
            <TaskItem key={task.id} task={task} onToggle={handleToggle}  />
          ))}
        </div>
      )}
    </>
  )
}

export default App
