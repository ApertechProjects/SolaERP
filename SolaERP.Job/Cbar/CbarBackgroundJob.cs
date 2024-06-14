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


namespace SolaERP.Job.Cbar
{
    [DisallowConcurrentExecution]
    public class CbarBackgroundJob : IJob
    {
        private readonly ILogger<EmailBackgroundJobIsSent> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        public CbarBackgroundJob(ILogger<EmailBackgroundJobIsSent> logger,
                                  IUnitOfWork unitOfWork,
                                  IMapper mapper
                                  )
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;

            string appsettingsFileName = AppSettingsHelper.GetAppSettingsFileName();
            IConfigurationBuilder builder = new ConfigurationBuilder()
            .AddJsonFile(appsettingsFileName, optional: true, reloadOnChange: false);

            _configuration = builder.Build();
        }

        public Task Execute(IJobExecutionContext context)
        {
            string url = $"https://www.cbar.az/currencies/{DateTime.Now.ToString("dd.MM.yyyy")}.xml";

            // Fetch XML data from URL
            string xmlData = FetchXmlDataAsync(url).GetAwaiter().GetResult();

            // Parse XML data
            var valutes = ParseXmlData(xmlData);

            using var bulkCopy = new SqlBulkCopy("Server=10.1.1.14;Database=SolaERP;User Id=sa;Password=D1g1t@l1z32000;MultipleActiveResultSets=True");

            bulkCopy.DestinationTableName = "Register.DailyRates";

            bulkCopy.ColumnMappings.Add(nameof(Valute.CurrencyCode), "CurrencyCode");
            bulkCopy.ColumnMappings.Add(nameof(Valute.Rate), "Rate");


            bulkCopy.WriteToServer(valutes.ConvertListOfCLassToDataTable());
            return Task.CompletedTask;
        }

        static async Task<string> FetchXmlDataAsync(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                // Get the XML data as a string
                string response = await client.GetStringAsync(url);
                return response;
            }
        }

        static List<Valute> ParseXmlData(string xmlData)
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

                        valute.Add(new Valute { CurrencyCode = code, Rate = value, Date = DateTime.Today.Date });
                    }
                    break;
                }
            }

            return valute;
        }

        public class Valute
        {
            public DateTime Date { get; set; }
            public string CurrencyCode { get; set; }
            public decimal Rate { get; set; }
        }
    }
}
