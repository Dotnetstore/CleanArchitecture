using Application.Common.Interfaces;
using Domain.Common;
using Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Common;

public sealed class GenericRepository<T> : IGenericRepository<T> where T : BaseAuditableEntity
{
    private readonly ApplicationDataContext _context;

    public GenericRepository(ApplicationDataContext context)
    {
        _context = context;
    }

    public IQueryable<T> Entities => _context.Set<T>();
    
    async Task<T?> IGenericRepository<T>.GetByIdAsync(Guid id)
    {
        return await _context.Set<T>().FindAsync(id);
    }

    async Task<List<T>> IGenericRepository<T>.GetAllAsync()
    {
        return await _context
            .Set<T>()
            .ToListAsync();
    }

    void IGenericRepository<T>.Create(T entity)
    {
        _context.Add(entity);
    }

    void IGenericRepository<T>.Update(T entity)
    {
        _context.Update(entity);
    }

    void IGenericRepository<T>.Delete(T entity)
    {
        _context.Remove(entity);
    }
}