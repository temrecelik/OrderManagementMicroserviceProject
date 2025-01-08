using AutoMapper;
using ProductService.BusinessLayer.Mapping.AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductService.BusinessLayer.Mapping
{
    public static class ObjectMapper
    {
        public static Lazy<IMapper> lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(opt =>
            {
                opt.AddProfile<ProductServiceMapping>();
            });
            return config.CreateMapper();
        });

        public static IMapper mapper => lazy.Value;
    }
}
