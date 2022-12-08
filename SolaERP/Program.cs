using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Serilog;
using SolaERP.Application.Identity_Server;
using SolaERP.Application.Mappers;
using SolaERP.Application.Services;
using SolaERP.Business.Models;
using SolaERP.Extensions;
using SolaERP.Infrastructure.Contracts.Services;
using SolaERP.Infrastructure.Entities.Auth;
using SolaERP.Infrastructure.ValidationRules;
using SolaERP.Middlewares;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add(new ValidationFilter()); })
    .AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    //This code ignores circular referanced object when they serialized to jsonfile 
}).Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.UseIdentityService();
builder.UseDataAccesServices();
builder.Services.AddEndpointsApiExplorer();
builder.UseValidationExtension();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.Configure<ApiBehaviorOptions>(config => { config.SuppressModelStateInvalidFilter = true; });
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
        corsBuilder => corsBuilder.WithOrigins(builder.Configuration["Cors:Origins"])
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .Build());
});
var logger = new LoggerConfiguration().WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DevelopmentConnectionString"),"logs").Enrich.FromLogContext().MinimumLevel.Error().CreateLogger();
    //File("logs.txt").Enrich.FromLogContext().MinimumLevel.Error().CreateLogger();

builder.Host.UseSerilog(logger);

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
         IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
         NameClaimType = ClaimTypes.NameIdentifier,
         LifetimeValidator = (notBefore, expires, securityToken, validationParametrs) => expires != null ? expires > DateTime.UtcNow : false
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

    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    c.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
       { jwtSecurityScheme, Array.Empty<string>() }
    });
});
builder.Services.AddSingleton<ConfHelper>(new ConfHelper { DevelopmentUrl = builder.Configuration.GetConnectionString("DevelopmentConnectionString") });
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpLogging();
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandlerMiddleware<Program>(app.Services.GetRequiredService<ILogger<Program>>());
//app.UseEndpoints(endpoints => endpoints.MapHub<ChatHub>("/chatHub"));
//app.UseGlobalExceptionHandlerMiddleware();
app.MapControllers();
app.Run();
