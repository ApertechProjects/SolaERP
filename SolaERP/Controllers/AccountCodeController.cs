using iTextSharp.text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using SolaERP.Application.Contracts.Services;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    //[Authorize]
    public class AccountCodeController : CustomBaseController
    {
        private readonly IAccountCodeService _accountCodeService;
        public AccountCodeController(IAccountCodeService accountCodeService)
        {
            _accountCodeService = accountCodeService;
        }

        [HttpGet("{businessUnitId}")]
        public async Task<IActionResult> GetAccountCodes(int businessUnitId)
          => CreateActionResult(await _accountCodeService.GetAccountCodesByBusinessUnit(businessUnitId));


        [HttpGet]
        public async Task<string> GetData()
        {
            string url = "https://www.cbar.az/currencies/22.12.2023.xml";

            // Fetch XML data from URL
            string xmlData = await FetchXmlDataAsync(url);

            // Parse XML data
            ParseXmlData(xmlData);
            return await Task.FromResult(xmlData);
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

        static void ParseXmlData(string xmlData)
        {
            // Load the XML data into an XDocument
            XDocument xdoc = XDocument.Parse(xmlData);

            // Example: Extract nodes by name
            var nodes = xdoc.Descendants("ValType"); // Replace "NodeName" with your actual node name
            var cc = xdoc.LastNode;
            // Process the extracted nodes

            var node = nodes.Last().ToString();
            //var node2s = node.Descendants("ValType");
            //var tt = node.LastAttribute.Document;


            XmlDocument document = new XmlDocument();
            document.LoadXml(node);
            XmlNodeList valTypeList = document.GetElementsByTagName("ValType");
            List<Vall> valute = new List<Vall>();
            foreach (XmlElement valTypeElement in valTypeList)
            {
                string typeAttribute = valTypeElement.GetAttribute("Type");
                if (typeAttribute == "Xarici valyutalar")
                {
                    XmlNodeList valuteNodes = valTypeElement.GetElementsByTagName("Valute");
                    foreach (XmlElement valuteElement in valuteNodes)
                    {
                        string nominal = valuteElement.GetElementsByTagName("Nominal")[0].InnerText;
                        string code = valuteElement.GetAttribute("Code");
                        decimal value = decimal.Parse(valuteElement.GetElementsByTagName("Value")[0].InnerText, CultureInfo.InvariantCulture);


                        valute.Add(new Vall { Name = code, Nominal = nominal, Value = value });
                    }
                    break;
                }
            }
        }

        public class Vall
        {
            public string Nominal { get; set; }
            public string Name { get; set; }
            public decimal Value { get; set; }
        }
    }
}
