using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using GunterBar.Domain.Interfaces;

namespace GunterBar.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly GunterBarDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(GunterBarDbContext context)
    {
        _context = context;
    }

    public async Task BeginTransactionAsync()
    {
        _transaction = await _context.Database.BeginTransactionAsync();
    }

    public async Task CommitAsync()
    {
        try
        {
            await SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
            }
        }
        catch
        {
            await RollbackAsync();
            throw;
        }
    }

    public async Task RollbackAsync()
    {
        if (_transaction != null)
        {
            await _transaction.RollbackAsync();
        }
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _transaction?.Dispose();
        _context.Dispose();
    }
}
