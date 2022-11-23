using SolaERP.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Dtos.BusinessUnit
{
    public class BusinessUnitsAllDto 
    {
        public int Id { get; set; }
        public string BusinessUnitCode { get; set; }
        public string BusinessUnitName { get; set; }
        public string TaxId { get; set; }
        public string Address { get; set; }
        public string CountryCode { get; set; }
        public string FullName { get; set; }
        public string Position { get; set; }
    }
}
