using DevSync.Contexts;
using DevSync.Interfaces.Repositories;
using DevSync.Models;
using Microsoft.EntityFrameworkCore;

namespace DevSync.Repos;

public class UserRepository : IUserRepository
{

    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User> CreateUserAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> FindUserByEmailAsync(string email)
    {
        return await _dbContext.Users.Where(user => user.Email == email).FirstOrDefaultAsync();
    }

    public async Task<User?> FindUserByIdAsync(int userId)
    {
        return await _dbContext.Users.FindAsync(userId);
    }

    public async Task<IEnumerable<User>> FindUsersAsync()
    {
        return await _dbContext.Users.ToListAsync();
    }

    public async Task<IEnumerable<User>> FindUsersByIdsAsync(IEnumerable<int> userIds)
    {
        return await _dbContext.Users
            .Where(user => userIds.Contains(user.Id))
            .ToListAsync();
    }

    public async Task<User> UpdateUserAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<bool> DeleteUserAsync(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
        return true;

    }
}
