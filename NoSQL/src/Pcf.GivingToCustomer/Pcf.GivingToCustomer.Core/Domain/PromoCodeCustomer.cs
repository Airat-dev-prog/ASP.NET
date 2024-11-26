using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;

namespace Pcf.GivingToCustomer.Core.Domain
{
    public class PromoCodeCustomer : BaseEntity
    {
        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid PromoCodeId { get; set; }
        public virtual PromoCode PromoCode { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid CustomerId { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
