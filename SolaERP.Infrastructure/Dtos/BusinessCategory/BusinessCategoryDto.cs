using SolaERP.Application.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.BusinessCategory
{
    public class BusinessCategoryDto
    {
        public int BusinessCategoryId { get; set; }
        public string BusinessCategoryName { get; set; }
        public int BusinessSectorId { get; set; }
        public string BusinessCategoryCode { get; set; }
        public bool Active { get; set; } = false;
    }
}
