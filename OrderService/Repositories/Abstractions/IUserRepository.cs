using OrderService.Entities;

namespace OrderService.Repositories.Abstractions;

public interface IUserRepository
{
    public void CreateUser(User user);
    public void DeleteUser(User user);
    public Task<User> GetUserById(int id);
    public Task<User> GetUserByOriginalUserId(int originalUserId);
}
