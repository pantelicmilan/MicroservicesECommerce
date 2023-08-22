using AuthService.Entities;

namespace AuthService.Repositories.Abstractions;

public interface IUserRepository
{
    public Task CreateUserAsync(User user);
    public Task<bool> IsUsernameAndEmailUniqueAsync(string username, string email);
    public Task<User> GetUserByUsernameAsync(string username);
    public Task<User> GetUserByUserId(int id);
    public void EditUser(User user);
    public void DeleteUser(User user);
}
