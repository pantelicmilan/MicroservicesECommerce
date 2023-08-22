namespace CatalogService.Repositories.Abstractions;

public interface IUnitOfWork
{
    public Task SaveChangesAsync();
}
