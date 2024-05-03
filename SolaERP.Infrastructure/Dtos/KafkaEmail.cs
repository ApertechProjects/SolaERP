using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos
{
    public class KafkaEmail
    {
        public EmailTemplate EmailTemplateKey { get; set; }
        public ApproveStatus ApproveStatus { get; set; }
        public List<Person> Persons { get; set; }
        public int Sequence { get; set; }
        public string ReferenceNo { get; set; }
        public string Link { get; set; }
    }

    public class Person
    {
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
