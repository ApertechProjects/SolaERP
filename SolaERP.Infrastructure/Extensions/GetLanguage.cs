using SolaERP.Application.Entities.Language;
using SolaERP.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Extensions
{
    public static class GetLanguageExtension
    {
        public static Enums.Language GetLanguageEnumValue(this string language)
        {
            return language switch
            {
                "az" => Enums.Language.az,
                "en" => Enums.Language.en,
                //"ru" => Enums.Language.ru
            };
        }
    }
}
