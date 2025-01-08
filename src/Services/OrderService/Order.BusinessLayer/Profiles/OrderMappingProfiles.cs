using AutoMapper;
using Order.EntityLayer.Concrete;
using Order.EntityLayer.Dtos;
using Shared.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.BusinessLayer.Profiles
{
    public class OrderMappingProfiles : Profile
    {
       public OrderMappingProfiles()
        {
            CreateMap<EntityLayer.Concrete.Order, GetOrderDto>().ReverseMap();
            CreateMap<EntityLayer.Concrete.Order, CreateOrderDto>().ReverseMap();
            CreateMap<EntityLayer.Concrete.OrderItem, CreateOrderItemDto>().ReverseMap();
          
        }
    }
}
