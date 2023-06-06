using Microsoft.AspNetCore.Mvc;
using SolaERP.Application.Extensions;
using System.Data;
using System.Data.SqlClient;

namespace SolaERP.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HomeController : Controller
    {
        [HttpGet]
        public string GetSchema()
        {
            using (SqlConnection connection = new SqlConnection("Server=88.198.193.132;Database=SolaERP;User Id=SLUser;Password=Sluser2020;"))
            {
                DataTable table = new DataTable();
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("Select * from Procurement.VendorBankDetail", connection))   
                {
                    SqlDataAdapter dp = new SqlDataAdapter();
                    dp.SelectCommand = cmd;
                    dp.Fill(table);
                }

                return table.GetDataTableColumNames(@"C:\\Users\\User\\Desktop");
            }
        }
    }
}
