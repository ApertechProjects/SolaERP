using Newtonsoft.Json;
using SolaERP.Application.Entities.SupplierEvaluation;
using SolaERP.Application.Enums;
using SolaERP.Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Support
{
    public class SupportSaveDto
    {
        public string subject { get; set; }
        public string description { get; set; }
		public List<AttachmentSaveModel> Attachments { get; set; }

		[JsonIgnore]
		public int UserId { get; set; }
	}
}