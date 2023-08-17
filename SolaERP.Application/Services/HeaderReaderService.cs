using Microsoft.AspNetCore.Http;

namespace SolaERP.Persistence.Services;

public class HeaderReaderService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public string GetToken()
    {
        return GetHeaderByName("Authorization")[7..];
    }

    public string GetHeaderByName(string name)
    {
        return _httpContextAccessor.HttpContext.Request.Headers[name];
    }
}