using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Infrastructure.Entities.Translate
{
    public class Translate : BaseEntity
    {
        public Int64 TranslateId { get; set; }
        public string LanguageCode { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
    }
}
