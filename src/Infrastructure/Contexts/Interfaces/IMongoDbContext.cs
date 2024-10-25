using System.Linq.Expressions;
using MongoDB.Driver;

namespace Infrastructure.Contexts.Interfaces;

public interface IMongoDbContext
{
    Task InsertAsync<TDocument>(TDocument document);

    Task DeleteAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

    Task UpdateAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
        UpdateDefinition<TDocument> update);

    Task<List<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter);

    Task<List<TDocument>> FindPagedAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
        Expression<Func<TDocument, object>> orderBy,
        bool descending,
        int page,
        int pageSize);
}