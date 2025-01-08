using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.DtoLayer.CategoryDtos
{
    public class UpdateCategoryDto
    {
        public string CategoryId { get; set; }

        [BsonRepresentation(MongoDB.Bson.BsonType.String)]
        [BsonElement("Name")]
        public string CategoryName { get; set; }
    }
}
