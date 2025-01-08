using Stock.EntityLayer.Concrete;
using Stock.EntityLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Test.FakeDataGenerator
{
    public class FakeDataGenerator
    {
        public static EntityLayer.Concrete.Stock FakeStockEntity()
        {
            return new EntityLayer.Concrete.Stock { Id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728", ProductId = "9aa316f4-d166-4519-9cb7-bef83ccc6f76", Count = 10 };
        }

        public static StockDto FakeStockDto()
        {

            return new StockDto
            {
                ProductId = "9aa316f4-d166-4519-9cb7-bef83ccc6f76",
                Count = 10
            };

        }
        public static List<StockDto> FakeStockDtoList() 
        {
            return new List<StockDto>
            {
                 new StockDto { ProductId = "9aa316f4-d166-4519-9cb7-bef83ccc6f76", Count = 10 },
                 new StockDto { ProductId = "b0e0c282-4e46-4c39-b499-93953f3acdee", Count = 20 },
                 new StockDto { ProductId = "8c44800b-b116-45ee-beb3-ec925f0e3e36", Count = 30 },
                 new StockDto { ProductId = "dee1f1fb-ec89-424b-b04d-1318670d18c1", Count = 40 },

             };
                   
        }

        public static StockDto FakeAddStockDto()
        {
            return new StockDto { ProductId = "9aa316f4-d166-4519-9cb7-bef83ccc6f76", Count = 10 };

        }
       

        public static StockUpdateDto FakeUpdateStockDto()
        {
            return new StockUpdateDto { Id = "d6a682c3-9a3b-4c44-bd95-0e5f846ab728", ProductId = "9aa316f4-d166-4519-9cb7-bef83ccc6f76", Count = 10 };

        }
    }
}
