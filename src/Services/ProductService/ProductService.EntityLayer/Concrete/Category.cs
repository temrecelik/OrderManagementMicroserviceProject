using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core.Entities;

namespace ProductService.EntityLayer.Concrete
{
    public class Category : IEntity
    {

        public Category()
        {
            CategoryId = Guid.NewGuid().ToString();
        }
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement(Order = 0)]
        public string CategoryId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement("Name")]
        public string CategoryName { get; set; }
    }
}
