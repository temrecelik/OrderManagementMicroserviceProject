

using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.EntityLayer.Concrete
{
    public class OrderInbox : IEntity
    {
        [Key]
        public string IdempotenToken { get; set; }

        public bool Processed { get; set; } 
        
        public string Payload { get; set; } 


    }
}
