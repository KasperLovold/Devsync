import { useState, useEffect, useCallback } from 'react';
import Modal from '../components/Modal';
import {
  Project,
  Task,
  getProjectById,
  createTaskForProject,
  updateTaskById,
  deleteTaskById,
  updateTaskStatus
} from '../api/projectclient';
import { useParams } from 'react-router-dom';
import { toast } from 'react-hot-toast';

export default function ProjectDetails() {
  const { id } = useParams<{ id: string }>();
  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedTask, setSelectedTask] = useState<Task | null>(null);

  const fetchProjectData = useCallback(async () => {
    try {
      setLoading(true);
      if (!id) {
        throw new Error('Project ID is required');
      }
      const project = await getProjectById(id);
      setProject(project);
      setLoading(false);
    } catch (err) {
      setError('Failed to load project data');
      setLoading(false);
      console.error(err);
    }
  }, [id]);
  useEffect(() => {
    fetchProjectData();
  }, [fetchProjectData]);


  const openTaskModal = (task: Task) => {
    setSelectedTask(task);
    setIsModalOpen(true);
  };

  const closeModal = () => {
    setIsModalOpen(false);
    setSelectedTask(null);
  };

  const createTask = async (taskData: Omit<Task, 'id'>) => {
    try {
      if(!project) {
        throw new Error('Project not found');
      }
      console.log("[TASK DATA]", taskData);
      const response = await createTaskForProject(project.id, taskData.title,
                                            taskData.description, taskData.status);
      setProject(prev => ({
         ...prev!,
         tasks: [...prev!.tasks, response]
      }));
      closeModal();
    } catch (err) {
      console.error('Failed to create task:', err);
      toast.error('Failed to create task');
    }
  };

  const updateTask = async (taskId: string, taskData: Partial<Task>) => {
    try {
      if(!project) {
        throw new Error('Project not found');
      }

      if(!taskData.title || !taskData.description || !taskData.status)
        throw new Error('Title, description and status are required');
      const response = await updateTaskById(taskId, taskData.title!, taskData.description!, taskData.status!);
      setProject(prev => ({
        ...prev!,
        tasks: prev!.tasks.map(task =>
          task.id === taskId ? { ...task, ...response } : task
        )
      }));
      toast.success('Task updated successfully');
      closeModal();
    } catch (err) {
      toast.error('Failed to update task');
      console.error('Failed to update task:', err);
    }
  };

  const deleteTask = async (taskId: string) => {
    try {
      await deleteTaskById(taskId);
      setProject(prev => ({
        ...prev!,
        tasks: prev!.tasks.filter(task => task.id !== taskId)
      }));
      closeModal();
    } catch (err) {
      toast.error('Failed to delete task');
      console.error('Failed to delete task:', err);
    }
  };

  // Function to handle drag and drop
  const handleDragEnd = async (taskId: string, newStatus: 0 | 1 | 2) => {
    try {
      await updateTaskStatus(taskId, newStatus);
      setProject(prev => ({
        ...prev!,
        tasks: prev!.tasks.map(task =>
          task.id === taskId ? { ...task, status: newStatus } : task
        )
      }));
    } catch (err) {
      toast.error('Failed to update task status');
      console.error('Failed to update task status:', err);
    }
  };

  // Loading state
  if (loading) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <svg className="animate-spin h-10 w-10 text-indigo-600 mx-auto" xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24">
            <circle className="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" strokeWidth="4"></circle>
            <path className="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4zm2 5.291A7.962 7.962 0 014 12H0c0 3.042 1.135 5.824 3 7.938l3-2.647z"></path>
          </svg>
          <p className="mt-2 text-gray-600">Loading project...</p>
        </div>
      </div>
    );
  }

  // Error state
  if (error || !project) {
    return (
      <div className="min-h-screen flex items-center justify-center bg-gray-50">
        <div className="text-center">
          <svg className="h-12 w-12 text-red-500 mx-auto" fill="none" viewBox="0 0 24 24" stroke="currentColor">
            <path strokeLinecap="round" strokeLinejoin="round" strokeWidth={2} d="M12 9v2m0 4h.01m-6.938 4h13.856c1.54 0 2.502-1.667 1.732-3L13.732 4c-.77-1.333-2.694-1.333-3.464 0L3.34 16c-.77 1.333.192 3 1.732 3z" />
          </svg>
          <h2 className="mt-2 text-lg font-medium text-gray-900">Failed to load project</h2>
          <p className="mt-1 text-gray-600">{error || "Could not retrieve project data"}</p>
          <button
            className="mt-4 px-4 py-2 bg-indigo-600 text-white rounded-md shadow-sm hover:bg-indigo-700"
            onClick={fetchProjectData}
          >
            Try Again
          </button>
        </div>
      </div>
    );
  }

  // Group tasks by status
  const todoTasks = project.tasks.filter(task => task.status === 0);
  const inProgressTasks = project.tasks.filter(task => task.status === 1);
  const completedTasks = project.tasks.filter(task => task.status === 2);

  return (
    <div className="min-h-screen bg-gray-50">
      {/* Project Header */}
      <header className="bg-white shadow">
        <div className="max-w-7xl mx-auto px-4 py-6 sm:px-6 lg:px-8">
          <div className="flex justify-between items-center">
            <div>
              <h1 className="text-3xl font-bold text-gray-900">{project.name}</h1>
              <p className="mt-1 text-sm text-gray-600">{project.description}</p>
            </div>
            <button
              className="px-4 py-2 bg-indigo-600 text-white rounded-md shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500"
              onClick={() => {
                // Create a new empty task and open modal
                const newTask: Omit<Task, 'id'> = {
                  title: '',
                  description: '',
                  status: 1,
                  assignee: null,
                  dueDate: null
                };
                setSelectedTask(newTask as Task);
                setIsModalOpen(true);
              }}
            >
              Add Task
            </button>
          </div>
        </div>
      </header>

      {/* Kanban Board */}
      <main className="max-w-7xl mx-auto px-4 py-6 sm:px-6 lg:px-8">
        <div className="grid grid-cols-1 md:grid-cols-3 gap-6">
          {/* To Do Column */}
          <div className="bg-white rounded-lg shadow">
            <div className="p-4 border-b border-gray-200 bg-gray-50 rounded-t-lg">
              <h2 className="text-lg font-medium text-gray-900">To Do</h2>
              <p className="text-sm text-gray-600">{todoTasks.length} tasks</p>
            </div>
            <div
              className="p-4 space-y-3 min-h-64"
              onDragOver={(e) => {
                e.preventDefault();
                e.dataTransfer.dropEffect = "move";
              }}
              onDrop={(e) => {
                e.preventDefault();
                const taskId = e.dataTransfer.getData("taskId");
                handleDragEnd(taskId, 0);
              }}
            >
              {todoTasks.map(task => (
                <div
                  key={task.id}
                  className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm hover:shadow cursor-pointer"
                  onClick={() => openTaskModal(task)}
                  draggable
                  onDragStart={(e) => {
                    e.dataTransfer.setData("taskId", task.id);
                    e.dataTransfer.effectAllowed = "move";
                  }}
                >
                  <h3 className="font-medium text-gray-900">{task.title}</h3>
                  <p className="text-sm text-gray-600 line-clamp-2 mt-1">{task.description}</p>
                  {task.assignee && (
                    <div className="mt-2 flex items-center">
                      <span className="inline-block h-6 w-6 rounded-full bg-indigo-100 text-indigo-800 text-xs flex items-center justify-center">
                        {task.assignee?.userName.charAt(0)}
                      </span>
                      <span className="ml-2 text-sm text-gray-600">{task.assignee?.userName}</span>
                    </div>
                  )}
                  {task.dueDate && (
                    <p className="mt-2 text-xs text-gray-500">Due: {new Date(task.dueDate).toLocaleDateString()}</p>
                  )}
                </div>
              ))}
              {todoTasks.length === 0 && (
                <div className="text-center py-8 text-gray-500">
                  <p>No tasks to do</p>
                </div>
              )}
            </div>
          </div>

          {/* In Progress Column */}
          <div className="bg-white rounded-lg shadow">
            <div className="p-4 border-b border-gray-200 bg-gray-50 rounded-t-lg">
              <h2 className="text-lg font-medium text-gray-900">In Progress</h2>
              <p className="text-sm text-gray-600">{inProgressTasks.length} tasks</p>
            </div>
            <div
              className="p-4 space-y-3 min-h-64"
              onDragOver={(e) => {
                e.preventDefault();
                e.dataTransfer.dropEffect = "move";
              }}
              onDrop={(e) => {
                e.preventDefault();
                const taskId = e.dataTransfer.getData("taskId");
                handleDragEnd(taskId, 1);
              }}
            >
              {inProgressTasks.map(task => (
                <div
                  key={task.id}
                  className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm hover:shadow cursor-pointer"
                  onClick={() => openTaskModal(task)}
                  draggable
                  onDragStart={(e) => {
                    e.dataTransfer.setData("taskId", task.id);
                    e.dataTransfer.effectAllowed = "move";
                  }}
                >
                  <h3 className="font-medium text-gray-900">{task.title}</h3>
                  <p className="text-sm text-gray-600 line-clamp-2 mt-1">{task.description}</p>
                  {task.assignee && (
                    <div className="mt-2 flex items-center">
                      <span className="inline-block h-6 w-6 rounded-full bg-indigo-100 text-indigo-800 text-xs flex items-center justify-center">
                        {task.assignee?.userName.charAt(0)}
                      </span>
                      <span className="ml-2 text-sm text-gray-600">{task.assignee?.userName}</span>
                    </div>
                  )}
                  {task.dueDate && (
                    <p className="mt-2 text-xs text-gray-500">Due: {new Date(task.dueDate).toLocaleDateString()}</p>
                  )}
                </div>
              ))}
              {inProgressTasks.length === 0 && (
                <div className="text-center py-8 text-gray-500">
                  <p>No tasks in progress</p>
                </div>
              )}
            </div>
          </div>

          {/* Completed Column */}
          <div className="bg-white rounded-lg shadow">
            <div className="p-4 border-b border-gray-200 bg-gray-50 rounded-t-lg">
              <h2 className="text-lg font-medium text-gray-900">Completed</h2>
              <p className="text-sm text-gray-600">{completedTasks.length} tasks</p>
            </div>
            <div
              className="p-4 space-y-3 min-h-64"
              onDragOver={(e) => {
                e.preventDefault();
                e.dataTransfer.dropEffect = "move";
              }}
              onDrop={(e) => {
                e.preventDefault();
                const taskId = e.dataTransfer.getData("taskId");
                handleDragEnd(taskId, 2);
              }}
            >
              {completedTasks.map(task => (
                <div
                  key={task.id}
                  className="bg-white border border-gray-200 rounded-lg p-4 shadow-sm hover:shadow cursor-pointer"
                  onClick={() => openTaskModal(task)}
                  draggable
                  onDragStart={(e) => {
                    e.dataTransfer.setData("taskId", task.id);
                    e.dataTransfer.effectAllowed = "move";
                  }}
                >
                  <h3 className="font-medium text-gray-900">{task.title}</h3>
                  <p className="text-sm text-gray-600 line-clamp-2 mt-1">{task.description}</p>
                  {task.assignee && (
                    <div className="mt-2 flex items-center">
                      <span className="inline-block h-6 w-6 rounded-full bg-indigo-100 text-indigo-800 text-xs flex items-center justify-center">
                        {task.assignee?.userName.charAt(0)}
                      </span>
                      <span className="ml-2 text-sm text-gray-600">{task.assignee?.userName}</span>
                    </div>
                  )}
                  {task.dueDate && (
                    <p className="mt-2 text-xs text-gray-500">Due: {new Date(task.dueDate).toLocaleDateString()}</p>
                  )}
                </div>
              ))}
              {completedTasks.length === 0 && (
                <div className="text-center py-8 text-gray-500">
                  <p>No completed tasks</p>
                </div>
              )}
            </div>
          </div>
        </div>
      </main>

      {/* Task Detail Modal */}
      <Modal isOpen={isModalOpen} onClose={closeModal}>
        {selectedTask && (
          <div className="space-y-4">
            <div>
              {selectedTask.id ? (
                <h2 className="text-xl font-bold text-gray-900">Edit Task</h2>
              ) : (
                <h2 className="text-xl font-bold text-gray-900">Create New Task</h2>
              )}
            </div>

            <div className="space-y-4">
              <div>
                <label htmlFor="title" className="block text-sm font-medium text-gray-700">Title</label>
                <input
                  type="text"
                  id="title"
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                  value={selectedTask.title}
                  onChange={(e) => setSelectedTask({...selectedTask, title: e.target.value})}
                />
              </div>

              <div>
                <label htmlFor="description" className="block text-sm font-medium text-gray-700">Description</label>
                <textarea
                  id="description"
                  rows={4}
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                  value={selectedTask.description}
                  onChange={(e) => setSelectedTask({...selectedTask, description: e.target.value})}
                />
              </div>

              <div>
                <label htmlFor="status" className="block text-sm font-medium text-gray-700">Status</label>
                <select
                  id="status"
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                  value={selectedTask.status}
                  onChange={(e) => setSelectedTask({...selectedTask, status: parseInt(e.target.value) as 1 | 2 | 3})}
                >
                  <option value={0}>To Do</option>
                  <option value={1}>In Progress</option>
                  <option value={2}>Completed</option>
                </select>
              </div>

              <div>
                <label htmlFor="assignee" className="block text-sm font-medium text-gray-700">Assignee</label>
                <input
                  type="text"
                  id="assignee"
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                  value={selectedTask.assignee?.userName || ''}
                />
              </div>

              <div>
                <label htmlFor="dueDate" className="block text-sm font-medium text-gray-700">Due Date</label>
                <input
                  type="date"
                  id="dueDate"
                  className="mt-1 block w-full rounded-md border-gray-300 shadow-sm focus:border-indigo-500 focus:ring-indigo-500"
                  value={selectedTask.dueDate || ''}
                  onChange={(e) => setSelectedTask({...selectedTask, dueDate: e.target.value})}
                />
              </div>
            </div>

            <div className="pt-4 flex justify-between">
              {selectedTask.id ? (
                <>
                  <button
                    type="button"
                    className="inline-flex justify-center rounded-md border border-transparent bg-red-600 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-red-700 focus:outline-none focus:ring-2 focus:ring-red-500 focus:ring-offset-2"
                    onClick={() => deleteTask(selectedTask.id)}
                  >
                    Delete
                  </button>
                  <button
                    type="button"
                    className="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                    onClick={() => updateTask(selectedTask.id, selectedTask)}
                  >
                    Save Changes
                  </button>
                </>
              ) : (
                <>
                  <button
                    type="button"
                    className="inline-flex justify-center rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 shadow-sm hover:bg-gray-50 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                    onClick={closeModal}
                  >
                    Cancel
                  </button>
                  <button
                    type="button"
                    className="inline-flex justify-center rounded-md border border-transparent bg-indigo-600 px-4 py-2 text-sm font-medium text-white shadow-sm hover:bg-indigo-700 focus:outline-none focus:ring-2 focus:ring-indigo-500 focus:ring-offset-2"
                    onClick={() => createTask(selectedTask)}
                  >
                    Create Task
                  </button>
                </>
              )}
            </div>
          </div>
        )}
      </Modal>
    </div>
  );
}
