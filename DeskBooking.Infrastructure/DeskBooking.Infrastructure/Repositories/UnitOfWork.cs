using System.Collections.Concurrent;
using DeskBooking.Domain.Interfaces;

namespace DeskBooking.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly DataContext _context;
    // Słownik, który będzie przechowywał instancje repozytoriów w pamięci
    private readonly ConcurrentDictionary<string, object> _repositories;

    public UnitOfWork(DataContext context)
    {
        _context = context;
        _repositories = new ConcurrentDictionary<string, object>();
    }

    public IRepository<T> Repository<T>() where T : class
    {
        var type = typeof(T).Name;

        return (IRepository<T>)_repositories.GetOrAdd(type, _ => new Repository<T>(_context));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public void Dispose()
    {
        _context.Dispose();
        GC.SuppressFinalize(this);
    }
}