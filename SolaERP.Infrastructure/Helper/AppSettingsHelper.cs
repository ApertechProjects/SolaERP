using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolaERP.Application.Helper
{
    public static class AppSettingsHelper
    {
        public static string GetAppSettingsFileName()
        {
            switch (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"))
            {
                case "Development":
                    return "appsettings.Development.json";
                case "Production":
                    return "appsettings.Production.json";
                case "GlDevelopment":
                    return "appsettings.GlDevelopment.json";
                default:
                    return "appsettings.Development.json";
            }
        }

        public static string GetApp()
        {
            var aa = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return aa;
        }
    }
}
