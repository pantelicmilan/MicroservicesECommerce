using AuthService.DataAccess;
using AuthService.Entities;
using AuthService.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace AuthService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MSSQLDataAccess _dataAccess;
    public UserRepository(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task CreateUserAsync(User user)
    {
        await _dataAccess.Users.AddAsync(user);
    }

    public async Task<User> GetUserByUserId(int id)
    {
       var user = await _dataAccess.Users.FirstOrDefaultAsync(u => u.Id == id);
       return user;
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        var user = await _dataAccess.Users.FirstOrDefaultAsync(u => u.Username == username);
        return user;
    }

    public async Task<bool> IsUsernameAndEmailUniqueAsync(string username, string email)
    {
       var userWithSameParameter =  await _dataAccess.Users
            .FirstOrDefaultAsync(par => par.Email == email || par.Username == username);
        if(userWithSameParameter == null)
        {
            return true;
        }
        return false;
    }

    public void EditUser(User user)
    {
        _dataAccess.Users.Update(user);
    }

    public void DeleteUser(User user)
    {
        _dataAccess.Users.Remove(user);
    }
}
