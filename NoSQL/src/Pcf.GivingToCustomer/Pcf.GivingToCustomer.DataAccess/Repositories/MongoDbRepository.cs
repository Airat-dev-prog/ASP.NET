using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Abstractions.Repositories;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.DataAccess.Data;

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
/*Удалить
            switch (typeof(T).Name)
            {
                case "Customer":
                    _collection = mongoDatabase.GetCollection<T>(mongoDbConfiguration.Value.CustomerCollectionName);
                    break;
                case "Preference":
                    _collection = mongoDatabase.GetCollection<T>(mongoDbConfiguration.Value.PreferenceCollectionName);
                    break;
                case "PromoCode":
                    _collection = mongoDatabase.GetCollection<T>(mongoDbConfiguration.Value.PromoCodeCollectionName);
                    break;
            }
*/
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
            await _collection.DeleteOneAsync(x => x.Id == entity.Id);
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
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetRangeByIdsAsync(List<Guid> ids)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateAsync(T entity)
        {
            throw new NotImplementedException();
        }
    }
}
