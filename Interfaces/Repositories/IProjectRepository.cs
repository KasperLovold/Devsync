using DevSync.Models;

namespace DevSync.Interfaces.Repositories;

public interface IProjectRepository
{
    public Task<IEnumerable<Project>> GetAllUserProjects(int userId);
    public Task<Project?> GetProjectById(Guid projectId);
    public Task<Project> CreateProject(Project project);
    public Task<Project> UpdateProject(Project project);
    public Task<TaskItem?> CreateTaskItem(TaskItem taskItem, Guid projectId);
    
    public Task<bool> UserHasAccessToProject(int userId, Guid projectId);
    public Task<bool> DeleteProject(Project project);
}