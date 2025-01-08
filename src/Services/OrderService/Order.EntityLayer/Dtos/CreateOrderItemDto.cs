﻿
using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.EntityLayer.Dtos
{
    public class CreateOrderItemDto :IDto
    {

        public string ProductId { get; set; }
        public int Count { get; set; }
        
    }
}
