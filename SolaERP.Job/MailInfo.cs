using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Job
{
    public class MailInfo
    {
        public string emailTemplateKey { get; set; }
        public List<Person> persons { get; set; }
        public string referenceNo { get; set; }
        public string link { get; set; }
        public string companyName { get; set; }
        public HashSet<RowInfo> rowInfos { get; set; }

    }

    public class Person
    {
        public string userName { get; set; }
        public string email { get; set; }
        public string lang { get; set; }
    }

    public class PersonDraft
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Language { get; set; }
    }

    public class RowInfo
    {
        public int id { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public string localDateTime { get; set; }
        public string reasonDescription { get; set; }
        public int sequence { get; set; }
        public string procedure { get; set; }
        public string comment { get; set; }
        public string approveStatus { get; set; }
    }

    public class RowInfoDraft
    {
        public int id { get; set; }
        public int notificationSenderId { get; set; }
        public string number { get; set; }
        public string name { get; set; }
        public string localDateTime { get; set; }
        public string reasonDescription { get; set; }
        public int sequence { get; set; }
        public string procedure { get; set; }
        public string comment { get; set; }
        public string approveStatus { get; set; }
    }
}
