using DevSync.Contexts;
using DevSync.Interfaces;
using DevSync.Interfaces.Repositories;
using DevSync.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSync.Repos;

public class ProjectRepository : IProjectRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<Project>> GetAllUserProjects(int userId) =>
        await _dbContext.Project
            .Where(project => project.Members.Any(x => x.UserId == userId))
            .ToListAsync();

    public async Task<Project?> GetProjectById(Guid projectId) =>
        await _dbContext.Project.Include(p => p.Members)
            .ThenInclude(p => p.User)
            .Include(p => p.Tasks)
            .FirstOrDefaultAsync(p => p.Id == projectId);

    public async Task<Project> CreateProject(Project project)
    {
        _dbContext.Project.Add(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }

    public async Task<TaskItem?> CreateTaskItem(TaskItem taskItem, Guid projectId)
    {
        var project = await GetProjectById(projectId);
        if (project == null) return null;
        
        taskItem.ProjectId = projectId;

        _dbContext.TaskItem.Add(taskItem);
        await _dbContext.SaveChangesAsync();
        return taskItem;
    }

    public async Task<bool> UserHasAccessToProject(int userId, Guid projectId)
    {
        var project = await _dbContext.Project.Where(p => projectId == p.Id)
            .Include(p => p.Members)
            .ThenInclude(p => p.User)
            .Where(u => userId == u.Members.Select(x => x.UserId).FirstOrDefault())
            .ToListAsync();
        return project.Count != 0;
    }


    public async Task<Project> UpdateProject(Project project)
    {
        _dbContext.Project.Update(project);
        await _dbContext.SaveChangesAsync();
        return project;
    }

    public async Task<bool> DeleteProject(Project project)
    {
        if(!await _dbContext.Project.AnyAsync(p => p.Id == project.Id)) return false;
        _dbContext.Project.Remove(project);
        await _dbContext.SaveChangesAsync();
        return true;
    }
}