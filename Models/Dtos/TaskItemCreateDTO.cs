
namespace DevSync.Models.DTOs;

public class TaskItemCreateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Status { get; set; } = 0;
}
