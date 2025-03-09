using DevSync.Interfaces.Services;
using DevSync.Models;
using DevSync.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace DevSync.Controllers;



[ApiController]
[Route("api/[controller]")]
public class TaskController : Controller
{
    private readonly ITaskService _taskService;
    private readonly IProjectService _projectService;
    private readonly IClaimsService _claimsService;
    
    public TaskController(ITaskService taskService, IProjectService projectService, IClaimsService claimsService)
    {
        _taskService = taskService;
        _projectService = projectService;
        _claimsService = claimsService;
    }
    
    [HttpPut("project/{projectId:guid}")]
    [Authorize]
    public async Task<IActionResult> AddTask([FromBody] TaskItemCreateDTO taskItemCreateDto, Guid projectId)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var project  = await _projectService.GetProjectById(projectId);
        if (project == null) return NotFound(new { Message = "Project not found." });
        
        var task = new TaskItem
        {
            ProjectId = project.Id,
            Title = taskItemCreateDto.Title,
            Description = taskItemCreateDto.Description,
            Status = TaskItemStatus.InProgress,
        };

        var createdTask = await _taskService.CreateTask(task);
        return Ok(new TaskItemResponseDTO
        {
            Id = createdTask.Id,
            Title = createdTask.Title,
            Description = createdTask.Description,
            Status = createdTask.Status
        });
    }
    
    [HttpGet("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Show(Guid id) => 
        await _taskService.GetTaskById(id) switch
        {
            null => NotFound(new { Message = "Task not found." }),
            var task => Ok(task)
        };

    [HttpGet("project/{id:guid}")]
    [Authorize]
    public async Task<IActionResult> Index(Guid id)
    {
        var userId = _claimsService.GetUserId();
        
        return await _projectService.GetProjectById(id) switch {
            null => NotFound(new { Message = "Project not found." }),
            _ when userId is null || !await _projectService.UserHasAccessToProject(userId.Value, id) => Unauthorized(),
            _ => Ok(await _taskService.GetAllProjectTasks(id))
        };
    }

    [HttpPut("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateTask([FromBody] TaskItemUpdateDTO item, Guid id)
    {
        var userId = _claimsService.GetUserId();
        var validation = await _taskService.ValidateTaskUpdate(id, userId, item);

        if (!validation.IsValid) return validation.ErrorResult!;

        await _taskService.UpdateTask(validation.Value!);
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userId = _claimsService.GetUserId();
        if(userId is null  || !await _projectService.UserHasAccessToProject(userId.Value, id)) return Unauthorized();
        if (await _taskService.DeleteTask(id)) return NoContent();
        return NotFound(new { Message = "Task not found." });
    }
}