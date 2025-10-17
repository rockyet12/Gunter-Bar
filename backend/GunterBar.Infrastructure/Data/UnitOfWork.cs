using GunterBar.Domain.Interfaces;

namespace GunterBar.Infrastructure.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly GunterBarDbContext _context;

    public UnitOfWork(GunterBarDbContext context)
    {
        _context = context;
    }

    public async Task CommitAsync()
    {
        await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
