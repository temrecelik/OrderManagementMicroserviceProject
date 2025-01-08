using Shared.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CompanyService.EntityLayer.Concrete
{
    public class CompanyOutbox : IEntity
    {
        [Key]
        public string IdempotenToken { get; set; }

        public DateTime OccuredON { get; set; }

        public DateTime? ProcessedDate { get; set; }

        public string Type { get; set; }

        public string Payload { get; set; }
    }
}

