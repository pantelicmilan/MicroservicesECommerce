using AuthService.DataAccess;
using AuthService.Migrations;
using AuthService.Repositories.Abstractions;

namespace AuthService.Repositories;

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
