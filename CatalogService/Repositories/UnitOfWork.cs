using CatalogService.DataAccess;
using CatalogService.Repositories.Abstractions;

namespace CatalogService.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly MSSQLDataAccess _dataAccess;

    public UnitOfWork(MSSQLDataAccess dataAccess)
    {
        _dataAccess = dataAccess;
    }

    public async Task SaveChangesAsync()
    {
        await _dataAccess.SaveChangesAsync();
    }
}
