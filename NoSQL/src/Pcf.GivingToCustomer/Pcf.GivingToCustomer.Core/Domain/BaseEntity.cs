using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Pcf.GivingToCustomer.Core.Domain
{
    public class BaseEntity
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        [BsonGuidRepresentation(GuidRepresentation.Standard)]
        public Guid Id { get; set; }
    }
}