using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Entities.UserReport
{
    public class UserReportFileAccess:BaseEntity
    {
        public int? Id { get; set; }
        public int UserId { get; set; }
        public string ReportFileName { get; set; }
        public string ReportFileId { get; set; }
    }
}
