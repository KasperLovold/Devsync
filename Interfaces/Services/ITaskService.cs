using DevSync.Common;
using DevSync.Models;
using DevSync.Models.DTOs;

namespace DevSync.Interfaces.Services;

public interface ITaskService
{
   public Task<List<TaskItem>> GetAllProjectTasks(Guid projectId); 
   public Task<TaskItem?> GetTaskById(Guid taskId);
   public Task<TaskItem> CreateTask(TaskItem task);
   public Task<TaskItem> UpdateTask(TaskItem task);
   public Task<bool> DeleteTask(Guid taskId);
   public Task<ValidationResult<TaskItem>> ValidateTaskUpdate(Guid taskId, int? userId, TaskItemUpdateDTO item);
}