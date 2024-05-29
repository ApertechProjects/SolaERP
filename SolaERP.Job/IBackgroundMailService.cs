using SolaERP.Application.Enums;
using SolaERP.Job.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
    public interface IBackgroundMailService
    {
        Task SendMail(List<RowInfo> rowInfos, Person person);
    }
}
