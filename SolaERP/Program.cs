using Microsoft.OpenApi.Models;
using SolaERP.Application.Mappers;
using SolaERP.Application.Services;
using SolaERP.DataAccess.Abstract;
using SolaERP.DataAccess.DataAcces.Implementation;
using SolaERP.DataAccess.Factories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddTransient<IUnitOfWork, SqlUnitOfWork>();
builder.Services.AddTransient<UserService, UserService>();
builder.Services.AddTransient<IUserRepository, SqlUserRepository>();
builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.AddTransient((t) =>
{
    var connectionString = builder.Configuration.GetConnectionString("DevelopmentConnectionString");
    return ConnectionFactory.CreateSqlConnection(connectionString);
});

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc($"v1", new OpenApiInfo
    {
        Title = "Our Title",
        Version = "v1",
        Description = "Our test swagger client",
    });
});

var app = builder.Build();
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
