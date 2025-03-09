namespace DevSync.Models.DTOs;

public class ProjectResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<ProjectMemberDto> Members { get; set; } = [];
    public List<TaskItemResponseDTO> Tasks { get; set; } = [];
}

public class ProjectMemberDto
{
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public ProjectRole Role { get; set; }
}