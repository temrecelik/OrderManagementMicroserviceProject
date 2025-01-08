using MongoDB.Bson.Serialization.Attributes;
using ProductService.DtoLayer.CategoryDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.DtoLayer.ProductDtos
{
    public class ResultProductDto
    {
        public string ProductId { get; set; }

        [BsonElement("Name")]
        public string ProductName { get; set; }

        [BsonElement("Description")]
        public string ProductDescription { get; set; }

        [BsonElement("Price")]
        public decimal ProductPrice { get; set; }


        [BsonRepresentation(MongoDB.Bson.BsonType.String)]

        [BsonElement("CategoryId")]
        public string CategoryId { get; set; }
       
    }
}
