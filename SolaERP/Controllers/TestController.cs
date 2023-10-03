using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace SolaERP.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TestController : ControllerBase
{
    [HttpGet("{count:int}")]
    public IActionResult GetUsers(int count)
    {
        var user = new User
        {
            EmployeeID = 1,
            FullName = "Nancy Davolio",
            Position = "Sales Representative",
            TitleOfCourtesy = "Ms.",
            BirthDate = new DateTime(1968, 12, 8),
            HireDate = new DateTime(2011, 5, 1),
            Address = "507 - 20th Ave. E.\r\nApt. 2A",
            City = "Seattle",
            Region = "WA",
            PostalCode = "98122",
            Country = "USA",
            HomePhone = "(206) 555-9857",
            Extension = "5467",
            Photo = "https://js.devexpress.com/Demos/WidgetsGallery/JSDemos/images/employees/06.png",
            Notes =
                "Education includes a BA in psychology from Colorado State University in 1990. She also completed \"The Art of the Cold Call.\" Nancy is a member of Toastmasters International.",
            ReportsTo = 2
        };
        var users = new List<User>();
        for (int i = 0; i < count; i++)
        {
            users.Add(user);
        }

        var response = new
        {
            users
        };

        var json = JsonConvert.SerializeObject(response);

        return Content(json, "application/json");
    }
}

public class User
{
    public int EmployeeID { get; set; }
    public string FullName { get; set; }
    public string Position { get; set; }
    public string TitleOfCourtesy { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime HireDate { get; set; }
    public string Address { get; set; }
    public string City { get; set; }
    public string Region { get; set; }
    public string PostalCode { get; set; }
    public string Country { get; set; }
    public string HomePhone { get; set; }
    public string Extension { get; set; }
    public string Photo { get; set; }
    public string Notes { get; set; }
    public int? ReportsTo { get; set; }
}