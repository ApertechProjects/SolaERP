using System.Text.RegularExpressions;

namespace SolaERP.API.Methods
{
    public static class Helper
    {
        public static string GetVerifyToken(string token)
        {
            var newtoken = Guid.NewGuid();
            string resultToken = newtoken + token;
            resultToken = Regex.Replace(resultToken, @"[^a-zA-Z0-9_.~\-]", "");
            return resultToken;
        }
    }
}
