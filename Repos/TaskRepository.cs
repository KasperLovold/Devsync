using DevSync.Contexts;
using DevSync.Interfaces;
using DevSync.Interfaces.Repositories;
using DevSync.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSync.Repos;

public class TaskRepository(ApplicationDbContext dbContext) : ITaskRepository
{
    public async Task<List<TaskItem>> GetAllProjectTasks(Guid projectId) =>
        await dbContext.TaskItem.Where(t => t.ProjectId == projectId).ToListAsync();

    public async Task<TaskItem?> GetTaskById(Guid taskId) =>
        await dbContext.TaskItem.SingleOrDefaultAsync(t => t.Id == taskId);

    public async Task<TaskItem> CreateTask(TaskItem task)
    {
        dbContext.TaskItem.Add(task);
        await dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<TaskItem> UpdateTask(TaskItem task)
    {
        dbContext.TaskItem.Update(task);
        await dbContext.SaveChangesAsync();
        return task;
    }

    public async Task<bool> DeleteTask(Guid taskId)
    {
        var task = await dbContext.TaskItem.SingleOrDefaultAsync(t => t.Id == taskId);
        if (task == null) return false;
        dbContext.TaskItem.Remove(task);
        await dbContext.SaveChangesAsync();
        return true;
    }
}