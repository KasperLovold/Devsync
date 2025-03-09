using DevSync.Models;

namespace DevSync.Interfaces.Services;

public interface IProjectService
{
    public Task<IEnumerable<Project>> GetAllUserProjects(int userId);
    public Task<Project?> GetProjectById(Guid projectId);
    public Task<Project> CreateProject(Project project);
    public Task<Project> UpdateProject(Project project);
    public Task<TaskItem?> CreateTaskItem(TaskItem taskItem, Guid projectId);
    public Task<bool> UserHasAccessToProject(int userId, Guid projectId);
    public Task<bool> DeleteProject(Project project);
}