import './App.css'
import { TaskForm } from './components/TaskForm';
import { TaskItem } from './components/TaskItem';
import { useTasks } from './hooks/useTasks';

function App() {
  const { data: tasks, isLoading, error } = useTasks();
  return (
    <>
      <TaskForm />

      {!isLoading && !error && tasks && tasks.length > 0 && (
        <div className="divide-y divide-gray-200">
          {tasks.map((task) => (
            <TaskItem key={task.id} task={task} />
          ))}
        </div>
      )}
    </>
  )
}

export default App
