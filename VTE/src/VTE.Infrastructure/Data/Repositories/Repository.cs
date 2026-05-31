namespace VTE.Infrastructure.Data.Repositories;

using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VTE.Core.Entities;
using VTE.Core.Interfaces;

public class Repository<T> : IRepository<T> where T : BaseEntity
{
    protected readonly VteDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public Repository(VteDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<T?> GetByIdAsync(long id, CancellationToken ct = default)
        => await _dbSet.FindAsync([id], ct);

    public async Task<List<T>> GetAllAsync(CancellationToken ct = default)
        => await _dbSet.ToListAsync(ct);

    public async Task<List<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => await _dbSet.Where(predicate).ToListAsync(ct);

    public async Task<T> AddAsync(T entity, CancellationToken ct = default)
    {
        await _dbSet.AddAsync(entity, ct);
        return entity;
    }

    public void Update(T entity) => _dbSet.Update(entity);

    public void Delete(T entity) => _dbSet.Remove(entity);
}

public class UnitOfWork : IUnitOfWork
{
    private readonly VteDbContext _context;

    public UnitOfWork(VteDbContext context) => _context = context;

    public async Task<int> SaveChangesAsync(CancellationToken ct = default)
        => await _context.SaveChangesAsync(ct);

    public void Dispose() => _context.Dispose();
}
