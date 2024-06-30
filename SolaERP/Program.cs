using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using RabbitMQ.Client;
using Serilog;
using SolaERP.Application.Validations;
using SolaERP.Extensions;
using SolaERP.Middlewares;
using SolaERP.Persistence.Mappers;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.FileProviders;
using SolaERP.Job;
using Quartz;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
    {
        options.Filters.Add(new ValidationFilter());
        options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
    }).AddJsonOptions(options => { options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles; })
    .Services
    .AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();

builder.UseIdentityService();
builder.ConfigureServices();
builder.UseValidationExtension();
builder.Services.AddTransient(sp => new ConnectionFactory()
{
    Uri = new(builder.Configuration["FileOptions:URI"])
});


//builder.Services.AddRequestMailsForIsSent();
builder.Services.AddRequestMailsForIsSent2();
builder.Services.AddRequestMailsForIsSent3();
builder.Services.AddCbarData();
builder.Services.AddHttpContextAccessor();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.Configure<ApiBehaviorOptions>(config => { config.SuppressModelStateInvalidFilter = true; });
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsBuilder => corsBuilder
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowAnyOrigin()
        .Build());
});

var logger = new LoggerConfiguration()
    .WriteTo.MSSqlServer(builder.Configuration.GetConnectionString("DevelopmentConnectionString"), "logs")
    .Enrich.FromLogContext()
    .MinimumLevel.Error()
    .CreateLogger();


builder.Services
    .AddFluentEmail(builder.Configuration["Mail:Mail"])
    .AddRazorRenderer()
    .AddSmtpSender(builder.Configuration["Mail:Host"], Convert.ToInt32(builder.Configuration["Mail:Port"]));


builder.Services.Configure<ConnectionFactory>(option =>
{
    option.Uri = new Uri(builder.Configuration["FileOptions:URI"]);
    option.DispatchConsumersAsync = true;
});

builder.Services.Configure<FormOptions>(options => { options.ValueCountLimit = int.MaxValue; });

builder.Host.UseSerilog(logger);

builder.Services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
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
        IssuerSigningKey =
            new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Token:SecurityKey"])),
        NameClaimType = ClaimTypes.NameIdentifier,
        LifetimeValidator = (notBefore, expires, securityToken, validationParametrs) =>
            expires != null ? expires > DateTime.UtcNow : false
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

    c.DescribeAllParametersInCamelCase();


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
builder.Services.AddHttpContextAccessor();
builder.Services.AddSignalR();

builder.Services.AddControllers();


IFileProvider? fileProvider = builder.Environment.ContentRootFileProvider;
IConfiguration? configuration = builder.Configuration;
var app = builder.Build();


app.UseSwagger();
app.UseSwaggerUI(c =>
{
    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "Web API");
});

app.UseHttpLogging();
app.UseCors();
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandlerMiddleware(app.Services.GetRequiredService<ILogger<Program>>());
app.MapControllers();
app.Run();