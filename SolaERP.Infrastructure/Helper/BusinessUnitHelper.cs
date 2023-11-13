using SolaERP.Application.Contracts.Services;

namespace SolaERP.Application.Helper;

public class BusinessUnitHelper
{
    private readonly IBusinessUnitService _businessUnitService;

    public BusinessUnitHelper(IBusinessUnitService businessUnitService)
    {
        _businessUnitService = businessUnitService;
    }

    private async Task<string> GetFullConnection(int businessUnitId)
    {
        var businessUnitList = await _businessUnitService.GetBusinessUnitListConnections();
        var businessUnit = businessUnitList.SingleOrDefault(x => x.BusinessUnitId == businessUnitId);
        return businessUnit.ConnectionData;
    }

    private async Task<ConnectionData> GetConnectionData(int businessUnitId)
    {
        var connectionFullData = await GetFullConnection(businessUnitId);
        //[DB_HOST(0.0.0.0)].DB_NAME.DB_SCHEMA.BUSINESS_UNIT_NAME + _(underscore symbol)
        var splittedHostString = connectionFullData.Split("].");
        var connectionData = new ConnectionData
        {
            HostWithBrackets = splittedHostString[0] + "]"
        };

        var withoutHostSplitString = splittedHostString[1].Split(".");
        connectionData.DbName = withoutHostSplitString[0];
        connectionData.Schema = withoutHostSplitString[1];
        connectionData.BusinessUnitNameWithUnderscore = withoutHostSplitString[2];

        return connectionData;
    }

    // EXEC [DB_HOST].DB_NAME.DB_SCHEMA.PROCEDURE_NAME ..variables
    public string BuildQueryForIntegration(int businessUnitId, string queryWithoutExec)
    {
        var connectionData = GetConnectionData(businessUnitId).Result;
        return $"EXEC {connectionData.HostWithBrackets}.SolaERPIntegration.dbo.{queryWithoutExec}";
    }
}

public class ConnectionData
{
    public string HostWithBrackets { get; set; }
    public string DbName { get; set; }
    public string Schema { get; set; }
    public string BusinessUnitNameWithUnderscore { get; set; }
}