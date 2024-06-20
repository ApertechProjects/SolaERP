using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.UserReport
{
    public class UserReportSaveDto
    {
        public int Id { get; set; }
        public string ReportFileName { get; set; }
        public string ReportFileId { get; set; }
        public List<int> Users { get; set; }
    }
}
