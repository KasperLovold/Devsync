using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace DevSync.Models.DTOs;

public class UserDto
{
    [EmailAddress, Required]
    public required string Email { get; set; }
    [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage = "Only alphanumeric characters are allowed.")]
    public  required string Username { get; set; }
    [Required, StringLength(50), MinLength(8), HiddenInput]
    public required string Password { get; set; }
}