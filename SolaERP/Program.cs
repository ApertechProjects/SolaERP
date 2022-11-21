using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.OpenApi.Models;
using SolaERP.Application.Identity_Server;
using SolaERP.Application.Mappers;
using SolaERP.Application.Services;
using SolaERP.Extensions;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.Services;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.Configure<KeyManagementOptions>(options =>
{
    options.XmlEncryptor = null;
});

builder.Services.AddIdentity<User, Role>().AddDefaultTokenProviders();
builder.Services.AddTransient<ITokenHandler, JwtTokenHandler>();
builder.Services.AddSingleton<IUserStore<User>, UserStore>();
builder.Services.AddSingleton<IRoleStore<Role>, RoleStore>();
builder.Services.AddSingleton<IPasswordHasher<User>, CustomPasswordHasher>();
builder.Services.AddEndpointsApiExplorer();
builder.UseSqlDataAccessServices();
builder.Services.AddAutoMapper(typeof(MapProfile));


builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
 .AddJwtBearer(options =>
 {
     options.RequireHttpsMetadata = false;
     options.SaveToken = true;
     options.TokenValidationParameters = new()
     {
         ValidateAudience = false,
         ValidateIssuer = false,
         ValidateLifetime = true,
         ValidateIssuerSigningKey = true,
         ValidAudience = builder.Configuration["Token:Audience"],
         ValidIssuer = builder.Configuration["Token:Issuer"],
         IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"]))
     };
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
