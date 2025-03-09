using DevSync.Models;

namespace DevSync.Interfaces.Repositories;

public interface ITaskRepository
{
   public Task<List<TaskItem>> GetAllProjectTasks(Guid projectId); 
   public Task<TaskItem?> GetTaskById(Guid taskId);
   public Task<TaskItem> CreateTask(TaskItem task);
   public Task<TaskItem> UpdateTask(TaskItem task);
   public Task<bool> DeleteTask(Guid taskId);
   
}