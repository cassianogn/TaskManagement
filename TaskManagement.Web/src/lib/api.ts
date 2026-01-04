import axios from 'axios';

export const api = axios.create({
  baseURL: 'https://localhost:7064/api/TaskItem',
  headers: {
    'Content-Type': 'application/json',
  },
});

