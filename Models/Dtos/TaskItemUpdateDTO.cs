namespace DevSync.Models.DTOs;

public class TaskItemUpdateDTO
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int? Assignee { get; set; }
    public TaskItemStatus Status { get; set; }
}
