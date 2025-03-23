namespace DevSync.Models;

public class TaskItem
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public TaskItemStatus Status { get; set; }
    public Guid ProjectId { get; set; }
    public Project? Project { get; set; }
    public User? Assignee { get; set; }
}

public enum TaskItemStatus
{
    Pending = 0,
    InProgress = 1,
    Completed = 2,
}
