using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Bson;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.DataAccess.Data;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Pcf.GivingToCustomer.DataAccess.Repositories
{
    public class MongoDbRepository<T>
        : IRepository<T>
        where T : BaseEntity

    {
        private readonly IMongoCollection<T> _collection;

        public MongoDbRepository(IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            var mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName);
            _collection = mongoDatabase.GetCollection<T>(typeof(T).Name + "s" );


            //Create Index
            var indexKeysDefinition = Builders<T>.IndexKeys.Ascending(x => x.Id);
            var indexOptions = new CreateIndexOptions();
            indexOptions.Unique = true;
            indexOptions.Name = typeof(T)+"_Id";
            _collection.Indexes.CreateOne(new CreateIndexModel<T>(indexKeysDefinition, indexOptions));
        }

        public async Task AddAsync(T entity)
        {
            try
            {
                await _collection.InsertOneAsync(entity);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey &&
                    ex.WriteError.Code == 11000)
                {
                    throw ;
                }

                throw;
            }
        }

        public async Task DeleteAsync(T entity)
        {
            try
            {
                await _collection.DeleteOneAsync(x => x.Id == entity.Id);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task<T> GetFirstWhere(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).SingleOrDefaultAsync();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            return await _collection.Find(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await _collection.Find(predicate).ToListAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            try
            {
                await _collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
            }
            catch (MongoWriteException ex)
            {
                if (ex.WriteError.Category == ServerErrorCategory.DuplicateKey &&
                    ex.WriteError.Code == 11000)
                {
                    throw;
                }

                throw;
            }
        }
    }
}
