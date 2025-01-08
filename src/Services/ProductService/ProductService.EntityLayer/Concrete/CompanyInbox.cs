using MongoDB.Bson.Serialization.Attributes;
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.EntityLayer.Concrete
{
    public class CompanyInbox :IEntity
    { 
        [BsonId]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]

         [BsonElement("IdempotenToken")]
         public string IdempotenToken { get; set; }

        [BsonElement("Processed")]
        public bool Processed { get; set; }

       [BsonElement("Payload")]
        public string Payload { get; set; }

    }
}
