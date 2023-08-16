using SolaERP.Application.Entities.SupplierEvaluation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Bid
{
    public class BidCardDto
    {
        public BidMainLoadDto BidMain { get; set; }

        public List<DeliveryTerms> DeliveryTermsList { get; set; }
        public List<PaymentTerms> PaymentTermsList { get; set; }
    }
}
