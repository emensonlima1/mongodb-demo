using System.Linq.Expressions;
using Infrastructure.Contexts.Interfaces;
using MongoDB.Driver;

namespace Infrastructure.Contexts;

public class MongoDbContext : IMongoDbContext
{
    private IMongoClient _client;
    private IMongoDatabase _database;

    public MongoDbContext(string databaseName, string connectionString = "mongodb://localhost:27017")
    {
        try
        {
            _client ??= new MongoClient(connectionString);
            _database = _client.GetDatabase(databaseName);
        }
        catch (Exception exception)
        {
            throw;
        }
    }

    private IMongoCollection<TDocument> GetCollection<TDocument>()
    {
        try
        {
            return _database.GetCollection<TDocument>(typeof(TDocument).Name);
        }
        catch (Exception e)
        {
            throw;
        }
    }

    public async Task InsertAsync<TDocument>(TDocument document)
    {
        try
        {
            var collection = GetCollection<TDocument>();
            await collection.InsertOneAsync(document);
        }
        catch (Exception exception)
        {
            throw;
        }
    }

    public async Task DeleteAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
    {
        try
        {
            var collection = GetCollection<TDocument>();
            await collection.DeleteOneAsync(filter);
        }
        catch (Exception exception)
        {
            throw;
        }
    }

    public async Task UpdateAsync<TDocument>(Expression<Func<TDocument, bool>> filter,
        UpdateDefinition<TDocument> update)
    {
        try
        {
            var collection = GetCollection<TDocument>();
            await collection.UpdateOneAsync(filter, update);
        }
        catch (Exception exception)
        {
            throw;
        }
    }

    public async Task<List<TDocument>> FindAsync<TDocument>(Expression<Func<TDocument, bool>> filter)
    {
        try
        {
            var collection = GetCollection<TDocument>();
            return await collection.Find(filter).ToListAsync();
        }
        catch (Exception exception)
        {
            throw;
        }
    }

    public async Task<List<TDocument>> FindPagedAsync<TDocument>
    (
        Expression<Func<TDocument, bool>> filter,
        Expression<Func<TDocument, object>> orderBy,
        bool descending,
        int page,
        int pageSize
    )
    {
        try
        {
            var collection = GetCollection<TDocument>();

            var skip = (page - 1) * pageSize;

            var findOptions = descending
                ? collection.Find(filter).SortByDescending(orderBy)
                : collection.Find(filter).SortBy(orderBy);

            var result = await findOptions
                .Skip(skip)
                .Limit(pageSize)
                .ToListAsync();

            return result;
        }
        catch (Exception exception)
        {
            throw;
        }
    }
}