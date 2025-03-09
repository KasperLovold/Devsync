using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using DevSync.Models;
namespace DevSync.Contexts;
public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
{
    public required DbSet<User> Users {get;set;}
    public required DbSet<Project> Project {get;set;}
    public required DbSet<TaskItem> TaskItem {get;set;}
    public required DbSet<Team> Team {get;set;}
    public required DbSet<ProjectMember> ProjectMember {get;set;}

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Email).HasMaxLength(524);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(255);
            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.Status).HasConversion<int>();
            entity.HasMany(p => p.Tasks)
                .WithOne(p => p.Project)
                .HasForeignKey(p => p.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(p => p.Members)
                .WithOne(p => p.Project)
                .HasForeignKey(p => p.ProjectId);
        });

        modelBuilder.Entity<ProjectMember>(entity =>
        {
            entity.Property(e => e.Role).HasConversion<int>();
            entity.HasKey(e => new { e.ProjectId, e.UserId });
            entity.HasOne(e => e.Project).WithMany(p => p.Members).HasForeignKey(e => e.ProjectId);
            entity.HasOne(e => e.User).WithMany().HasForeignKey(e => e.UserId);
        });

        modelBuilder.Entity<TaskItem>(entity =>
        {
            entity.Property(e => e.Status).HasConversion<int>();
            entity.Property(e => e.Description).HasMaxLength(1024);
            entity.Property(e => e.Title).HasMaxLength(255);
        });

        modelBuilder.Entity<Team>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(255);
        });
    }
    
}