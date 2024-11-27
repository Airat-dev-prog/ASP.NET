using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pcf.GivingToCustomer.Core.Domain;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization;
using SharpCompress.Common;
using MongoDB.Bson;
using MongoDB.Driver.Core.Configuration;

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

            mongoClient.DropDatabase(mongoDatabase.DatabaseNamespace.DatabaseName);
        }

        public void InitializeDb()
        {

            var PreferenceCollection = mongoDatabase.GetCollection<Preference>("Preferences");
            var CustomerCollection = mongoDatabase.GetCollection<Customer>("Customers");

            try
            {
                PreferenceCollection.InsertMany([.. FakeDataFactory.Preferences]);
                CustomerCollection.InsertMany([.. FakeDataFactory.Customers]);
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
            catch(BsonSerializationException ex)
            {
                //throw;
            }
        }
    }
}
