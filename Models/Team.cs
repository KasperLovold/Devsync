namespace DevSync.Models;

public class Team
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public ICollection<Project> Projects { get; set; } = new List<Project>();
    public ICollection<User> Users { get; set; } = new List<User>();
}