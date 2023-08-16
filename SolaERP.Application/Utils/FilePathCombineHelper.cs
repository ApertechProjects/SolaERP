using SolaERP.Application.Enums;

namespace SolaERP.Persistence.Utils;

public static class FilePathCombineHelper
{
    private const string BASE_FILE_URL = "http://116.203.90.202:8080/api/v1/home";

    public static string CombinePath(Modules module, string fileName)
    {
        return string.IsNullOrEmpty(fileName)
            ? null
            : Path.Combine(BASE_FILE_URL, "module", module.ToString(), "fileName", fileName);
    }
}