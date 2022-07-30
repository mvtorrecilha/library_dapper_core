using Library.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Library.Core.Interfaces.Repositories;

public interface IBaseRepository<TEntity> where TEntity : BaseEntity
{
    Task<TEntity> GetAsync(Guid id);

    Task<IEnumerable<TEntity>> GetAllAsync();

    Task AddAsync(TEntity entity);

    Task DeleteAsync(TEntity entity);

    Task UpdateAsync(TEntity entity);
}
