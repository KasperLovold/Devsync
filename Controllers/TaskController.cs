using DevSync.Interfaces.Services;
using DevSync.Models;
using DevSync.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        var project = await _projectService.GetProjectById(projectId);
        if (project == null) return NotFound(new { Message = "Project not found." });

        var task = new TaskItem
        {
            ProjectId = project.Id,
            Title = taskItemCreateDto.Title,
            Description = taskItemCreateDto.Description,
            Status = (TaskItemStatus)taskItemCreateDto.Status
        };

        var createdTask = await _taskService.CreateTask(task);
        return CreatedAtAction(nameof(Show), new { id = createdTask.Id }, new TaskItemResponseDTO
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

        return await _projectService.GetProjectById(id) switch
        {
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

        return await validation.Match<Task<IActionResult>>(
            async validTask =>
            {
                var updateTask = await _taskService.UpdateTask(validTask);
                return Ok(new
                {
                    id = updateTask.Id,
                    title = updateTask.Title,
                    status = updateTask.Status,
                    description = updateTask.Description,
                });
            },
            Task.FromResult
        );
    }

    [HttpPatch("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> UpdateTaskStatus(Guid id, 
            [FromBody] TaskItemStatusUpdateDTO status)
    {
        var task = await _taskService.GetTaskById(id);
        if (task == null) return NotFound(new { Message = "Task not found." });

        task.Status = (TaskItemStatus)status.Status;
        var updatedTask = await _taskService.UpdateTask(task);
        return Ok(new
        {
            id = task.Id,
            title = task.Title,
            status = updatedTask.Status,
            description = task.Description,
        });
    }

    [HttpDelete("{id:guid}")]
    [Authorize]
    public async Task<IActionResult> DeleteTask(Guid id)
    {
        var userId = _claimsService.GetUserId();
        if (userId is null || !await _projectService.UserHasAccessToProject(userId.Value, id)) return Unauthorized();
        if (await _taskService.DeleteTask(id)) return NoContent();
        return NotFound(new { Message = "Task not found." });
    }
}
