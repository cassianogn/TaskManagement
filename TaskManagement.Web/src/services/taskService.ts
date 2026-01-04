import { api } from '../lib/api';

export interface TaskItem {
  id: string;
  title: string;
  isCompleted: boolean;
}

export interface CreateTaskRequest {
  title: string;
}

export interface CreateTaskResponse {
  id: string;
}

export const taskService = {
  getAll: async (): Promise<TaskItem[]> => {
    const response = await api.get<TaskItem[]>('/');
    return response.data;
  },

  create: async (data: CreateTaskRequest): Promise<CreateTaskResponse> => {
    const response = await api.post<CreateTaskResponse>('/', data);
    return response.data;
  },

  toggle: async (id: string): Promise<void> => {
    await api.patch(`/${id}/toggle`);
  },
};

