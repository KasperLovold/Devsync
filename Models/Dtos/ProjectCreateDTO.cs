using System.ComponentModel.DataAnnotations;

namespace DevSync.Models.DTOs;

public class ProjectCreateDto
{
    [Required, StringLength(128), MinLength(3)]
    public required string Name { get; set; }    
    [StringLength(1028)]
    public string? Description { get; set; }
}
