using Domain.Common.Interfaces;

namespace Application.Common.Interfaces;

public interface IGenericRepository<T> where T : class, IEntity
{
    IQueryable<T> Entities { get; }

    Task<T?> GetByIdAsync(Guid id);

    Task<List<T>> GetAllAsync();

    void Create(T entity);

    void Update(T entity);

    void Delete(T entity);
}