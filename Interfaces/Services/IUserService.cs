using DevSync.Models;
using Microsoft.AspNetCore.Identity;

namespace DevSync.Interfaces.Services;

public interface IUserService
{
   public Task<User> CreateUserAsync(User user); 
   public Task<User?> FindUserByEmailAsync(string email);
   public Task<User?> FindUserByIdAsync(int userId);
   public Task<IEnumerable<User>> FindUsersAsync();
   public Task<IEnumerable<User>> FindUsersByIdsAsync(IEnumerable<int> userIds);
   public Task<User> UpdateUserAsync(User user);
   public Task<bool> DeleteUserAsync(User user);
   public string HashPassword(User user, string password);
   public PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword,
      string providedPassword);
}