using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.Vendors
{
    public class Shipment : BaseEntity
    {
        public int ShipmentId { get; set; }
        public string ShipmentName { get; set; }
        public string ShipmentCode { get; set; }
    }
}
