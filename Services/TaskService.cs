using DevSync.Common;
using DevSync.Interfaces.Repositories;
using DevSync.Interfaces.Services;
using DevSync.Models;
using DevSync.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DevSync.Services;

public class TaskService(ITaskRepository taskRepository, IProjectService projectService, IUserService userService) : ITaskService
{
    public async Task<List<TaskItem>> GetAllProjectTasks(Guid projectId) => 
        await  taskRepository.GetAllProjectTasks(projectId);

    public async Task<TaskItem?> GetTaskById(Guid taskId) =>
        await  taskRepository.GetTaskById(taskId);

    public async Task<TaskItem> CreateTask(TaskItem task) => 
        await taskRepository.CreateTask(task);

    public async Task<TaskItem> UpdateTask(TaskItem task) =>
        await taskRepository.UpdateTask(task);

    public async Task<bool> DeleteTask(Guid taskId) =>
        await taskRepository.DeleteTask(taskId);
    
    public Task<ValidationResult<TaskItem>> ValidateTaskUpdate(Guid taskId, int? userId, TaskItemUpdateDTO item)
    {
        return ValidateTaskExists(taskId)
            .BindAsync(task => ValidateUserAccess(task, userId))
            .BindAsync(task => UpdateTaskProperties(task, item));

        async Task<ValidationResult<TaskItem>> ValidateTaskExists(Guid id)
        {
            var task = await GetTaskById(id);
            return task != null 
                ? ValidationResult<TaskItem>.Success(task)
                : ValidationResult<TaskItem>.Failure(new NotFoundObjectResult(new { Message = "Task not found." }));
        }

        async Task<ValidationResult<TaskItem>> ValidateUserAccess(TaskItem task, int? uid)
        {
            if (uid is null)
                return ValidationResult<TaskItem>.Failure(new UnauthorizedResult());
        
            var hasAccess = await projectService.UserHasAccessToProject(uid.Value, taskId);
            return hasAccess
                ? ValidationResult<TaskItem>.Success(task)
                : ValidationResult<TaskItem>.Failure(new UnauthorizedResult());
        }

        async Task<ValidationResult<TaskItem>> UpdateTaskProperties(TaskItem task, TaskItemUpdateDTO updateDto)
        {
            task.Title = updateDto.Title;
            task.Description = updateDto.Description;
            task.Status = updateDto.Status;
    
            if (updateDto.Assignee.HasValue)
            {
                task.Assignee = await userService.FindUserByIdAsync(updateDto.Assignee.Value);
            }
    
            return ValidationResult<TaskItem>.Success(task);
        }
    }
    
}