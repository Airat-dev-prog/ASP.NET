using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace Pcf.GivingToCustomer.Core.Domain
{
    public class PromoCode
        : BaseEntity
    {
        public string Code { get; set; }

        public string ServiceInfo { get; set; }

        public DateTime BeginDate { get; set; }

        public DateTime EndDate { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid PartnerId { get; set; }
        
        public virtual Preference Preference { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid PreferenceId { get; set; }
        
        public virtual ICollection<PromoCodeCustomer> Customers { get; set; }
    }
}