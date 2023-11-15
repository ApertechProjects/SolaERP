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
            bool isDevelopment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development";
            if (isDevelopment)
                return "appsettings.Development.json";
            else
                return "appsettings.Production.json";
        }

        public static string GetApp()
        {
            var aa = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return aa;
        }
    }
}
