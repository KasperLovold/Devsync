using DevSync.Models;
using Microsoft.AspNetCore.Identity;
using DevSync.Interfaces.Repositories;
using DevSync.Interfaces.Services;

namespace DevSync.Services;

public class UserService : IUserService
{
    private readonly PasswordHasher<User> _passwordHasher = new();
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }


    public async Task<User> CreateUserAsync(User user)
    {
        return await _userRepository.CreateUserAsync(user);
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await _userRepository.FindUserByEmailAsync(email);
    }

    public async Task<User?> FindUserByIdAsync(int userId)
    {
        return await _userRepository.FindUserByIdAsync(userId);
    }

    public async Task<IEnumerable<User>> FindUsersAsync()
    {
        return await _userRepository.FindUsersAsync();
    }

    public async Task<IEnumerable<User>> FindUsersByIdsAsync(IEnumerable<int> userIds)
    {
        return await _userRepository.FindUsersByIdsAsync(userIds);
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        return await _userRepository.UpdateUserAsync(user);
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        return await _userRepository.DeleteUserAsync(user);
    }

    public string HashPassword(User user, string password)
    {
        return _passwordHasher.HashPassword(user, password);
    }

    public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword,
        string providedPassword)
    {
        return _passwordHasher.VerifyHashedPassword(user, hashedPassword, providedPassword);
    }
}