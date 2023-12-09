using System.Collections;
using Application.Common.Interfaces;
using Infrastructure.Contexts;

namespace Infrastructure.Persistence.Common;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDataContext _context;
    private readonly Hashtable _repositories = new();
    private bool _isDisposed;
    
    public UnitOfWork(ApplicationDataContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
    
    IGenericRepository<T> IUnitOfWork.Repository<T>()
    {
        var type = typeof(T).Name;

        if (!_repositories.ContainsKey(type))
        {
            var repositoryType = typeof(GenericRepository<>);

            var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), _context);

            _repositories.Add(type, repositoryInstance);
        }

        return (IGenericRepository<T>) _repositories[type]!;
    }
    
    async Task<int> IUnitOfWork.SaveAsync(CancellationToken cancellationToken)
    {
        return await _context.SaveChangesAsync(cancellationToken);
    }

    void IUnitOfWork.Rollback()
    {
        _context.ChangeTracker.Entries().ToList().ForEach(x => x.Reload());
    }
    
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_isDisposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
        
        _isDisposed = true;
    }
}