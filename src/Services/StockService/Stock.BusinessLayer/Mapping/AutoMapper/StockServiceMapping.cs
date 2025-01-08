using AutoMapper;
using Stock.EntityLayer.Concrete;
using Stock.EntityLayer.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessLayer.Mapping.AutoMapper
{
    public class StockServiceMapping : Profile
    {
        public StockServiceMapping()
        {
            CreateMap<EntityLayer.Concrete.Stock, StockDto>().ReverseMap();
            CreateMap<EntityLayer.Concrete.Stock, StockUpdateDto>().ReverseMap();
        }
    }
}
