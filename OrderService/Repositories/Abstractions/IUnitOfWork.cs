namespace OrderService.Repositories.Abstractions;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}
