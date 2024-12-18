using SolaERP.Job.Enums;
using SolaERP.Job.RFQClose;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
    public interface ISend
    {
        Task SendRequestMails(StatusType statusType);
        Task SendRFQCloseMails();
    }
}
