using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.UserReport
{
    public class UserReportFileAccessDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string ReportFileName { get; set; }
        public int ReportFileId { get; set; }
    }
}
