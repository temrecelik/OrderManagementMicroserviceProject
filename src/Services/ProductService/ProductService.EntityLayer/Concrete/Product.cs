using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared.Core.Entities;

namespace ProductService.EntityLayer.Concrete
{
    public class Product : IEntity
    {

        public Product()
        {
            ProductId = Guid.NewGuid().ToString();
        }
        [BsonId]
        //[BsonGuidRepresentation(MongoDB.Bson.GuidRepresentation.CSharpLegacy)]
        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement(Order = 0)]
        public  string ProductId { get; set; }

        [BsonElement("Name")]
        public string ProductName { get; set; }

        [BsonElement("Description")]
        public string ProductDescription { get; set; }

        [BsonElement("Price")]
        public decimal ProductPrice { get; set; }

        [BsonElement("CompanyId")]
        public string CompanyId { get; set; }


        [BsonRepresentation(MongoDB.Bson.BsonType.String)]

        [BsonElement("CategoryId")]
        public string CategoryId { get; set; }

        [BsonIgnore]
        public Category Category { get; set; }
    }
}
