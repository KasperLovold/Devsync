using DevSync.Interfaces.Repositories;
using DevSync.Interfaces.Services;
using DevSync.Models;

namespace DevSync.Services;

public class ProjectService : IProjectService
{
    private readonly  IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
       _projectRepository = projectRepository; 
    }
    
    public async Task<IEnumerable<Project>> GetAllUserProjects(int userId) => 
        await _projectRepository.GetAllUserProjects(userId);

    public async Task<Project?> GetProjectById(Guid projectId) => await _projectRepository.GetProjectById(projectId);

    public async Task<Project> CreateProject(Project project) => await _projectRepository.CreateProject(project); 

    public async Task<Project> UpdateProject(Project project) => await _projectRepository.UpdateProject(project);
    public async Task<TaskItem?> CreateTaskItem(TaskItem taskItem, Guid projectId) =>
        await _projectRepository.CreateTaskItem(taskItem, projectId);

    public async Task<bool> UserHasAccessToProject(int userId, Guid projectId) =>
        await _projectRepository.UserHasAccessToProject(userId, projectId);


    public async Task<bool> DeleteProject(Project project) => await _projectRepository.DeleteProject(project);
}
