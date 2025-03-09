using System.Security.Claims;
using DevSync.Interfaces.Handlers;
using DevSync.Interfaces.Services;
using DevSync.Models;
using DevSync.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace DevSync.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : Controller
{
    private readonly ILogger<UserController> _logger;
    private readonly IUserService _userService;
    private readonly IJwtHandler _jwtHandler;

    public UserController(ILogger<UserController> logger, IUserService userService, IJwtHandler jwtHandler)
    {
        _userService = userService;
        _jwtHandler = jwtHandler;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserDto registerRequest)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
        
        var existingUser = await _userService.FindUserByEmailAsync(registerRequest.Email);
        if(existingUser != null) return Conflict(new { Message = "User with this email already exists." });

        var user = new User
        {
            Email = registerRequest.Email,
            UserName = registerRequest.Username,
        };
        
        var hashedPw = _userService.HashPassword(user, registerRequest.Password);
        user.PasswordHash = hashedPw;
        
        await _userService.CreateUserAsync(user);
        return Ok(new { Message = "User created successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if(!ModelState.IsValid) return BadRequest(ModelState);
       var user = await _userService.FindUserByEmailAsync(loginRequest.Email); 
       if(user?.PasswordHash == null) return Unauthorized();
       
       var pwIsCorrect = _userService.VerifyHashedPassword(user, user.PasswordHash, loginRequest.Password);
       if(pwIsCorrect is PasswordVerificationResult.Failed) return Unauthorized();

       var token = _jwtHandler.GenerateToken(user);
       if(token == null) return Unauthorized();

       return Ok(new { token = _jwtHandler.GenerateKey(token), iss = token.IssuedAt, expires = token.ValidTo });
    }

    [HttpGet("me")]
    [Authorize]
    public async Task<IActionResult> Me()
    {
        var userEmail = User.FindFirstValue(ClaimTypes.Email);
        if(string.IsNullOrEmpty(userEmail)) return Unauthorized(new { Message = "Invalid email address." });

        return await _userService.FindUserByEmailAsync(userEmail) switch
        {
            null => Unauthorized(new { Message = "User not found in the claim token" }),
            var user => Ok(new { userName = user.UserName, email = user.Email, phoneNumber = user.PhoneNumber })
        };
    }
}