import { api } from "./api";

export interface Member {
  userId: number;
  userName: string;
  role: number;
};

export interface Task {
  id: string;
  title: string;
  description: string;
  status: number;
  assignee: Member | null;
  dueDate: string | null;
}

export interface Project {
  id: string;
  name: string;
  description: string;
  createdAt: string;
  members: Array<Member>;
  tasks: Array<Task>;
}

export enum Status {
  Pending = 0,
  InProgress = 1,
  Completed = 2,
}

export const getProjects = async (): Promise<Project[]> => {
  const { data } = await api.get("/Project");
  return data;
};

export const createProject = async (name: string, description: string)
    : Promise<Project> => {
  const resp = await api.post("/Project", {
    name,
    description,
  });
  if (resp.status !== 201) {
    throw new Error("Failed to create project");
  }
  return resp.data;
}

export const getProjectById = async (id: string): Promise<Project> => {
  const { data } = await api.get(`/Project/${id}`);
  return data;
}

export const createTaskForProject = async (projectId: string, title: string,
                                           description: string, status: Status):
  Promise<Task> => {
  const resp = await api.put(`/Task/project/${projectId}`, {
    title,
    description,
    status
  });

  if (resp.status !== 201) {
    throw new Error("Failed to create task");
  }

  return resp.data;
}

export const updateTaskById = async (taskId: string, title: string,
         description: string, status: Status): Promise<Task> => {
  const resp = await api.put(`/Task/${taskId}`, {
    title,
    description,
    status,
  });

  if (resp.status !== 200) {
    throw new Error("Failed to update task");
  }

  return resp.data;
}

export const deleteTaskById = async (taskId: string): Promise<void> => {
  const resp = await api.delete(`/Task/${taskId}`);
  if (resp.status !== 204) {
    throw new Error("Failed to delete task");
  }
}

export const updateTaskStatus = async (taskId: string,
                                       taskItemStatus: Status):Promise<Task> => {
  console.log(taskId, taskItemStatus);
  const resp = await api.patch(`/Task/${taskId}`, {
    taskItemStatus,
  });

  if (resp.status !== 200) {
    throw new Error("Failed to update task status");
  }

  return resp.data;
}


