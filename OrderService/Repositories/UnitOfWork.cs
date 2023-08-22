using OrderService.DataAccess;
using OrderService.Repositories.Abstractions;

namespace OrderService.Repositories;

public class UnitOfWork: IUnitOfWork
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
