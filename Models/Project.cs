using System.ComponentModel.DataAnnotations;

namespace DevSync.Models;

public class Project
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    
    public int CreatorId { get; set; }
    public User? Creator { get; set; }
    public TaskStatus Status { get; set; }
    public ICollection<ProjectMember> Members { get; set; } = new List<ProjectMember>();
    public ICollection<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    
    [Timestamp]
    public byte[] RowVersion { get; set; } = [0]; 
}

public enum TaskStatus  {
   Created,
   InProgress,
   Completed,
}