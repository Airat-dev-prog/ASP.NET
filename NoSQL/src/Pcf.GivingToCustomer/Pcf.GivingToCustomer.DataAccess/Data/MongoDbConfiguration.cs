using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pcf.GivingToCustomer.DataAccess.Data
{
    public class MongoDbConfiguration
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string CustomerCollectionName { get; set; } = null!;
        public string PreferenceCollectionName { get; set; } = null!;
        public string PromoCodeCollectionName { get; set; } = null!;


    }
}
