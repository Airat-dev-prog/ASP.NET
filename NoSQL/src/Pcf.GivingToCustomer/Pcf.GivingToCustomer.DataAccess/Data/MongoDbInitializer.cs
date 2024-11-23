using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Domain;
using Microsoft.Extensions.Options;

namespace Pcf.GivingToCustomer.DataAccess.Data
{
    public class MongoDbInitializer
        : IDbInitializer
    {

        private readonly IMongoClient mongoClient;
        private readonly IMongoDatabase mongoDatabase ;

        public MongoDbInitializer(IOptions<MongoDbConfiguration> mongoDbConfiguration)
        {
            mongoClient = new MongoClient(mongoDbConfiguration.Value.ConnectionString);
            mongoDatabase = mongoClient.GetDatabase(mongoDbConfiguration.Value.DatabaseName);
        }
        public void InitializeDb()
        {
            var PreferenceCollection = mongoDatabase.GetCollection<Preference>("Preferences");
            //PreferenceCollection.InsertMany([.. FakeDataFactory.Preferences]);
            
            var CustomerCollection = mongoDatabase.GetCollection<Customer>("Customers");
            //CustomerCollection.InsertMany([.. FakeDataFactory.Customers]);
        }
    }
}
