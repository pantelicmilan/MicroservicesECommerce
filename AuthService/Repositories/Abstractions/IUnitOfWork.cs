namespace AuthService.Repositories.Abstractions;

public interface IUnitOfWork
{
    Task SaveChangesAsync();
}
