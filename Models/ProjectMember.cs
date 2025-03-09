namespace DevSync.Models;

public class ProjectMember
{
    public int UserId { get; set; }
    public Guid ProjectId { get; set; }
    public User? User { get; set; }
    public Project? Project { get; set; }
    public ProjectRole Role { get; set; } = ProjectRole.Member;
}

public enum ProjectRole
{
    Member,
    Admin,
    Owner
}