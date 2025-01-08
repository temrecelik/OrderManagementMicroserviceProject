using MongoDB.Bson.Serialization.Attributes;
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.EntityLayer.Concrete
{
     public  class ProductOutbox : IEntity
    {
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]

        [BsonElement("IdempotenToken")]
        public string IdempotenToken { get; set; }

        [BsonElement("OccuredON")]
        public DateTime OccuredON { get; set; }

        [BsonElement("ProcessedDate")]
        public DateTime? ProcessedDate { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Payload")]
        public string Payload { get; set; }
    }
}
