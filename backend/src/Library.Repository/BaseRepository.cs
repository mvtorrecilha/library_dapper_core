using Dapper.Contrib.Extensions;
using Library.Core.Interfaces.Factories;
using Library.Core.Interfaces.Repositories;
using Library.Core.Models;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Library.Repository;

public class BaseRepository<TEntity> : IBaseRepository<TEntity> where TEntity : BaseEntity
{
    protected readonly IConnectionFactory _connectionFactory;

    public BaseRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public virtual async Task AddAsync(TEntity entity)
    {
        await CreatePolicy().ExecuteAsync(async () =>
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.InsertAsync(entity);
        });
    }

    public virtual async Task DeleteAsync(TEntity entity)
    {
        await CreatePolicy().ExecuteAsync(async () =>
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.DeleteAsync(entity);
        });
    }

    public virtual async Task<TEntity> GetAsync(Guid id)
    {
        return await CreatePolicy().ExecuteAsync(async () =>
        {
            using var connection = _connectionFactory.GetOpenConnection();
            return await connection.GetAsync<TEntity>(id);
        });
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await CreatePolicy().ExecuteAsync(async () =>
        {
            using var connection = _connectionFactory.GetOpenConnection();
            return await connection.GetAllAsync<TEntity>();
        });
    }

    public virtual async Task UpdateAsync(TEntity entity)
    {
        await CreatePolicy().ExecuteAsync(async () =>
        {
            using var connection = _connectionFactory.GetOpenConnection();
            await connection.UpdateAsync(entity);
        });
    }

    protected AsyncRetryPolicy CreatePolicy(int retries = 5) =>
        Policy
            .Handle<SqlException>()
            .WaitAndRetryAsync(
                retryCount: retries,
                sleepDurationProvider: retry => TimeSpan.FromMilliseconds(new Random().Next(1, 300)));
}
