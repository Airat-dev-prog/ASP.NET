using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Pcf.GivingToCustomer.Core.Domain;
using Pcf.GivingToCustomer.DataAccess.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess
{
    public class Удалить_MongoDbStore
    {
        private readonly IMongoCollection<Customer> customerCollection;
        private readonly IMongoCollection<PromoCode> promoCodeCollection;
        private readonly IMongoCollection<Preference> preferenceCollection;

        public Удалить_MongoDbStore(IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            var mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName);

            customerCollection = mongoDatabase.GetCollection<Customer>(mongoDbConfiguration.Value.CustomerCollectionName);
            promoCodeCollection = mongoDatabase.GetCollection<PromoCode>(mongoDbConfiguration.Value.PromoCodeCollectionName);
            preferenceCollection = mongoDatabase.GetCollection<Preference>(mongoDbConfiguration.Value.PreferenceCollectionName);
        }
    }
}
