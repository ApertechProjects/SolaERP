using Microsoft.OpenApi.Models;
using SolaERP.Business.CommonLogic;
using SolaERP.Business.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v1", new OpenApiInfo
    {
        Title = "Our Title",
        Version = "v1",
        Description = "Our test swagger client",
    });
});
builder.Services.AddSingleton<ConfHelper>(new ConfHelper { DevelopmentUrl = builder.Configuration.GetConnectionString("DevelopmentConnectionString") });


var app = builder.Build();


//app.Run(
//{
//    using (SqlConnection connection = new SqlConnection("Server=88.198.193.132;Database=SolaERP;User Id=SLUser;Password=Sluser2020;"))
//    {
//        DataTable table = new DataTable();
//        connection.Open();
//        using (SqlCommand cmd = new SqlCommand("Select * from Config.AppUser", connection))
//        {
//            SqlDataAdapter dp = new SqlDataAdapter();
//            dp.SelectCommand = cmd;
//            dp.Fill(table);
//        }
//        table.GetDataTableColumNames(@"C:\\Users\\HP\\Desktop");
//    }

//});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
