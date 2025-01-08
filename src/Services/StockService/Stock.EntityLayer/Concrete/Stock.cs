using MongoDB.Bson.Serialization.Attributes;
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.EntityLayer.Concrete
{
    public class Stock : IEntity
    {
        public Stock()
        {
            Id = Guid.NewGuid().ToString();

        }

        [BsonId]
       
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement(Order = 0)]
        public string Id { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement("ProductId")]
        public string ProductId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.Int64)]
        [BsonElement("Count")]
        public int Count { get; set; }
    }
}
