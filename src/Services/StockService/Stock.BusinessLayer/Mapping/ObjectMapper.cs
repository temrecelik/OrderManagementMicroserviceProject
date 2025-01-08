using AutoMapper;
using Stock.BusinessLayer.Mapping.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.BusinessLayer.Mapping
{
    public  static class ObjectMapper
    {
        public static Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<StockServiceMapping>();
            });
            return config.CreateMapper();
        });

        public static IMapper mapper => lazy.Value;
    }
}
