using DevSync.Contexts;
using DevSync.Interfaces;
using DevSync.Interfaces.Repositories;
using DevSync.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSync.Repos;

public class TaskRepository : ITaskRepository
{
    private readonly ApplicationDbContext _dbContext;

    public TaskRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TaskItem>> GetAllProjectTasks(Guid projectId) =>
        await _dbContext.TaskItem.Where(t => t.ProjectId == projectId).ToListAsync();

    public async Task<TaskItem?> GetTaskById(Guid taskId) =>
        await _dbContext.TaskItem.SingleOrDefaultAsync(t => t.Id == taskId);

    public async Task<TaskItem> CreateTask(TaskItem task)
    {
        _dbContext.TaskItem.Add(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateTask(TaskItem task)
    {
        _dbContext.TaskItem.Update(task);
        await _dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTask(Guid taskId)
    {
        var task = await _dbContext.TaskItem.SingleOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return false;
        _dbContext.TaskItem.Remove(task);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}