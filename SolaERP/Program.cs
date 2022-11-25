using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => { options.Filters.Add(new ValidationFilter()); }).Services.AddFluentValidationAutoValidation().AddFluentValidationClientsideAdapters();
builder.Services.AddIdentity<User, Role>().AddDefaultTokenProviders();
builder.Services.AddTransient<ITokenHandler, JwtTokenHandler>();
builder.Services.AddScoped<IUserStore<User>, UserStore>();
builder.Services.AddSingleton<IRoleStore<Role>, RoleStore>();
builder.Services.AddSingleton<IPasswordHasher<User>, CustomPasswordHasher>();
builder.Services.AddEndpointsApiExplorer();
builder.UseSqlDataAccessServices();
builder.ValidationExtension();

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
builder.Services.Configure<ApiBehaviorOptions>(options => { options.SuppressModelStateInvalidFilter = true; });
builder.Host.UseSerilog();
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

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("CorsPolicy");
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseGlobalExceptionHandlerMiddleware();
app.MapControllers();
app.Run();
