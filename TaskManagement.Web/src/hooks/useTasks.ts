import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { taskService, type TaskItem } from '../services/taskService';

const TASKS_QUERY_KEY = ['tasks'] as const;

export const useTasks = () => {
  return useQuery({
    queryKey: TASKS_QUERY_KEY,
    queryFn: taskService.getAll,
  });
};

export const useCreateTask = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: taskService.create,
    onSuccess: () => {
      queryClient.invalidateQueries({ queryKey: TASKS_QUERY_KEY });
    },
  });
};

export const useToggleTask = () => {
  const queryClient = useQueryClient();

  return useMutation({
    mutationFn: taskService.toggle,
    onMutate: async (taskId: string) => {
      await queryClient.cancelQueries({ queryKey: TASKS_QUERY_KEY });

      const previousTasks = queryClient.getQueryData<TaskItem[]>(TASKS_QUERY_KEY);

      if (previousTasks) {
        queryClient.setQueryData<TaskItem[]>(TASKS_QUERY_KEY, (old?: TaskItem[]) => {
          if (!old) return old;
          return old.map((task) =>
            task.id === taskId ? { ...task, isCompleted: !task.isCompleted } : task
          );
        });
      }

      return { previousTasks };
    },
    onError: (_error, _taskId, context) => {
      if (context?.previousTasks) {
        queryClient.setQueryData(TASKS_QUERY_KEY, context.previousTasks);
      }
    },
    onSettled: () => {
      queryClient.invalidateQueries({ queryKey: TASKS_QUERY_KEY });
    },
  });
};

