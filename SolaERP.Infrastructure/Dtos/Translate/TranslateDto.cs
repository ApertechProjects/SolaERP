using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Dtos.Translate
{
    public class TranslateDto
    {
        public Int64 TranslateId { get; set; }
        public string LanguageCode { get; set; }
        public string Key { get; set; }
        public string Text { get; set; }
    }
}
