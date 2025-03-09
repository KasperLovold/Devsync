using DevSync.Models;

namespace DevSync.Interfaces.Repositories;

public interface IUserRepository
{
   public Task<User> CreateUserAsync(User user); 
   public Task<User?> FindUserByEmailAsync(string email);
   public Task<User?> FindUserByIdAsync(int userId);
   public Task<IEnumerable<User>> FindUsersAsync();
   public Task<IEnumerable<User>> FindUsersByIdsAsync(IEnumerable<int> userIds);
   public Task<User> UpdateUserAsync(User user);
   public Task<bool> DeleteUserAsync(User user);
}