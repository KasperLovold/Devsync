using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
namespace DevSync.Models;

public class User : IdentityUser<int>
{
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
}