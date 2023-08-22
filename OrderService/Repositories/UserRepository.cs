using Microsoft.EntityFrameworkCore;
using OrderService.DataAccess;
using OrderService.Entities;
using OrderService.Repositories.Abstractions;

namespace OrderService.Repositories;

public class UserRepository: IUserRepository
{
    private readonly MSSQLDataAccess _dataAccess;

    public UserRepository(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public void CreateUser(User user)
    {
        _dataAccess.Users.Add(user);
    }

    public async Task<User> GetUserById(int id)
    {
        var user = await _dataAccess.Users.FirstOrDefaultAsync(u => u.Id == id);
        return user;
    }   

    public async Task<User> GetUserByOriginalUserId(int originalUserId)
    {
        var user = await _dataAccess.Users.FirstOrDefaultAsync(u => u.OriginalUserId == originalUserId);
        return user;
    }

    public void DeleteUser(User user)
    {
        _dataAccess.Users.Remove(user);
    }

}
