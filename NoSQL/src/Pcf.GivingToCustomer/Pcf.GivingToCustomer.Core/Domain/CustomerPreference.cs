using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Pcf.GivingToCustomer.Core.Domain
{
    public class CustomerPreference
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid PreferenceId { get; set; }
        public virtual Preference Preference { get; set; }
    }
}