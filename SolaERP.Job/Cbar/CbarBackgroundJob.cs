using AutoMapper;
using Microsoft.Extensions.Logging;
using Quartz;
using SolaERP.Application.UnitOfWork;
using SolaERP.Job.EmailIsSent;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SolaERP.Application.Helper;
using System.Configuration;
using static SolaERP.Job.Cbar.CbarBackgroundJob;
using System.Data.Common;
using SolaERP.DataAccess.Extensions;
using System.Collections;
using System.Data;
using Serilog;


namespace SolaERP.Job.Cbar
{
    [DisallowConcurrentExecution]
    public class CbarBackgroundJob : IJob
    {
        private readonly ILogger<CbarBackgroundJob> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CbarBackgroundJob(ILogger<CbarBackgroundJob> logger,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper
                                  )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            string appsettingsFileName = "appsettings.Production.json";
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }

        private readonly DateTime date = DateTime.Now;

        private async Task<bool> CheckDataIsExist()
        {
            string commandText = @$"select * from Register.DailyRates where Date = @date";

            using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:DevelopmentConnectionString"]))
            using (SqlCommand cmd = new SqlCommand(commandText, cn))
            {
                cmd.Parameters.AddWithValue(cmd, "@date", DateTime.Now.ToString("yyyy-MM-dd"));

                cn.Open();
                var res = await cmd.ExecuteReaderAsync();
                if (await res.ReadAsync())
                {
                    cn.Close();
                    return true;

                }
                cn.Close();
                return false;
            }

        }

        public async Task Execute(IJobExecutionContext context)
        {
            try
            {
                if (!await CheckDataIsExist())
                {
                    //string url = $"https://www.cbar.az/currencies/{date.ToString("dd.MM.yyyy")}.xml";

                    //// Fetch XML data from URL
                    //string xmlData = await FetchXmlDataAsync(url);
                    //if (xmlData.Contains(date.ToString("dd.MM.yyyy")))
                    //{
                    //    // Parse XML data
                    //    var valutes = ParseXmlData(xmlData);

                    //    await BulkInsert(valutes);
                    //    await RunDailyCurrencyRate();
                    //}
                }
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, ex.Message);
            }
        }

        private async Task BulkInsert(List<Valute> valutes)
        {
            using var bulkCopy = new SqlBulkCopy(_configuration["ConnectionStrings:DevelopmentConnectionString"]);

            bulkCopy.DestinationTableName = "Register.DailyRates";

            bulkCopy.ColumnMappings.Add(nameof(Valute.Date), "Date");
            bulkCopy.ColumnMappings.Add(nameof(Valute.CurrencyCode), "CurrencyCode");
            bulkCopy.ColumnMappings.Add(nameof(Valute.Rate), "Rate");

            var data = valutes.ConvertListOfCLassToDataTable();
            await bulkCopy.WriteToServerAsync(data);
        }

        private async Task RunDailyCurrencyRate()
        {
            string commandText = @$"exec SP_RunDailyCurrencyRates_I '{date.ToString("yyyy-MM-dd")}'";
            try
            {
                using (SqlConnection cn = new SqlConnection(_configuration["ConnectionStrings:DevelopmentConnectionString"]))
                using (SqlCommand cmd = new SqlCommand(commandText, cn))
                {
                    //cmd.Parameters.AddWithValue(cmd, "@date", );

                    cn.Open();
                    var res = await cmd.ExecuteNonQueryAsync();

                    cn.Close();
                }
            }
            catch (Exception ex)
            {

            }

        }

        private async Task<string> FetchXmlDataAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                // Get the XML data as a string
                string response = await client.GetStringAsync(url);
                return response;
            }
        }

        private List<Valute> ParseXmlData(string xmlData)
        {
            // Load the XML data into an XDocument
            XDocument xdoc = XDocument.Parse(xmlData);

            // Example: Extract nodes by name
            var nodes = xdoc.Descendants("ValType"); // Replace "NodeName" with your actual node name

            var node = nodes.Last().ToString();

            XmlDocument document = new XmlDocument();
            document.LoadXml(node);
            XmlNodeList valTypeList = document.GetElementsByTagName("ValType");
            List<Valute> valute = new List<Valute>();
            foreach (XmlElement valTypeElement in valTypeList)
            {
                string typeAttribute = valTypeElement.GetAttribute("Type");
                if (typeAttribute == "Xarici valyutalar")
                {
                    XmlNodeList valuteNodes = valTypeElement.GetElementsByTagName("Valute");
                    foreach (XmlElement valuteElement in valuteNodes)
                    {
                        int nominal = Convert.ToInt32(valuteElement.GetElementsByTagName("Nominal")[0].InnerText);
                        string code = valuteElement.GetAttribute("Code");
                        decimal value = decimal.Parse(valuteElement.GetElementsByTagName("Value")[0].InnerText, CultureInfo.InvariantCulture);
                        if (nominal > 1)
                        {
                            value = value / nominal;
                        }

                        valute.Add(new Valute { CurrencyCode = code, Rate = value, Date = date.ToString("yyyy-MM-dd") });
                    }
                    break;
                }
            }

            return valute;
        }

        private class Valute
        {
            public string Date { get; set; }
            public string CurrencyCode { get; set; }
            public decimal Rate { get; set; }
        }
    }
}
