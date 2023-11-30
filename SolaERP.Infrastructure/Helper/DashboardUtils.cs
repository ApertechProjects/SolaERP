using DevExpress.DashboardAspNetCore;
using DevExpress.DashboardCommon;
using DevExpress.DashboardWeb;
using DevExpress.DataAccess.Json;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;

namespace SolaERP.Application.Helper;

public class DevExpressDashboardUtils
{
    public static DashboardConfigurator CreateDashboardConfigurator(IConfiguration configuration, IFileProvider fileProvider) {
        DashboardConfigurator configurator = new DashboardConfigurator();
        // configurator.SetDashboardStorage(new DashboardFileStorage(fileProvider.GetFileInfo("App_Data/Dashboards").PhysicalPath));
        configurator.SetDashboardStorage(new DashboardFileStorage("/media"));
        configurator.SetDataSourceStorage(CreateDataSourceStorage());
        configurator.SetConnectionStringsProvider(new DashboardConnectionStringsProvider(configuration));
        configurator.ConfigureDataConnection += Configurator_ConfigureDataConnection;
        return configurator;

    }
    static DataSourceInMemoryStorage CreateDataSourceStorage() {
        DataSourceInMemoryStorage dataSourceStorage = new DataSourceInMemoryStorage();
        DashboardJsonDataSource jsonDataSource = new DashboardJsonDataSource("DevExpressReporting");
        jsonDataSource.ConnectionName = "jsonDataSourceConnection";
        jsonDataSource.RootElement = "DevExpressReporting";
        dataSourceStorage.RegisterDataSource("jsonDataSourceSupport", jsonDataSource.SaveToXml());
        return dataSourceStorage;
    }
    static void Configurator_ConfigureDataConnection(object sender, ConfigureDataConnectionWebEventArgs e) {
        if (e.ConnectionName == "jsonDataSourceConnection") {
            Uri fileUri = new Uri("https://raw.githubusercontent.com/DevExpress-Examples/DataSources/master/JSON/customers.json");
            JsonSourceConnectionParameters jsonParams = new JsonSourceConnectionParameters();
            jsonParams.JsonSource = new UriJsonSource(fileUri);
            e.ConnectionParameters = jsonParams;
        }
    }
}