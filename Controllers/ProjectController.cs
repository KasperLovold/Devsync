using DevSync.Interfaces;
using DevSync.Interfaces.Services;
using DevSync.Models;
using DevSync.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevSync.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProjectController : Controller
{
    private readonly IProjectService _projectService;
    private readonly IClaimsService _claimsService;

    public ProjectController(IProjectService projectService, IClaimsService claimsService)
    {
        _projectService = projectService;
        _claimsService = claimsService;
    }
    
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetAllUserProjects()
    {
        var userId = _claimsService.GetUserId();
        if (userId == null) return Unauthorized(new { Message = "Invalid token" });

        var projects = await _projectService.GetAllUserProjects(userId.Value);
        return Ok(projects);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateProject([FromBody] ProjectCreateDto project)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        
        var userId = _claimsService.GetUserId();
        if (userId == null) return Unauthorized(new { Message = "Invalid token" });

        if (string.IsNullOrWhiteSpace(project.Name)) return BadRequest(new { Message = "Invalid name" });

        var newProject = new Project
        {
            Name = project.Name,
            Description = project.Description ?? string.Empty,
            CreatorId = userId.Value,
            Members = new List<ProjectMember>
            {
                new ProjectMember
                {
                    UserId = userId.Value,
                    Role = ProjectRole.Owner
                }
            }
        };
        
        var createdProject = await _projectService.CreateProject(newProject);
        
        return CreatedAtAction(nameof(GetProjectById), new { id = createdProject.Id }, new ProjectResponseDto
        {
            Id = createdProject.Id,
            Name = createdProject.Name,
            Description = createdProject.Description,
            CreatedAt = createdProject.CreatedAt
        });
    }
    
    
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var project = await _projectService.GetProjectById(id);
        if (project == null) return NotFound(new { Message = "Project not found." });
        
        var projectResp = new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            Members = project.Members.Select(m => new ProjectMemberDto
            {
                UserId = m.UserId,
                UserName = m.User?.UserName ?? string.Empty,
                Role = m.Role
            }).ToList(),
            Tasks =  project.Tasks.Select(t => new TaskItemResponseDTO
            {
                Id = t.Id,
                Title = t.Title,
                Description = t.Description,
                Status = t.Status,
            }).ToList()
        };
        
        return Ok(projectResp);
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> AddTaskToProject([FromBody] TaskItemCreateDTO taskItemCreateDto, Guid id)
    {
        var project  = await _projectService.GetProjectById(id);
        if (project == null) return NotFound(new { Message = "Project not found." });
        var task = new TaskItem
        {
            ProjectId = project.Id,
            Title = taskItemCreateDto.Title,
            Description = taskItemCreateDto.Description,
            Status = TaskItemStatus.InProgress,
        };

        return await _projectService.CreateTaskItem(task, id) switch
        {
            null => NotFound(new { Message = "Project not found." }),
            var createdTask => Ok(new TaskItemResponseDTO
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                Status = createdTask.Status
            })
        };    
    }
}