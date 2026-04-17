using System.Text;
using ESG.Api.Common;
using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Repository;
using ESG.Api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
Console.WriteLine($"Current Environment: {env.EnvironmentName}");

// Get MySQL password from environment variables (SECURE)
string mysqlPassword = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD") ?? "";

Console.WriteLine($"Password: {mysqlPassword}");
if (string.IsNullOrWhiteSpace(mysqlPassword))
{
    Console.WriteLine("Warning: MYSQL_ROOT_PASSWORD is not set. Ensure it is configured correctly.");
}

// Get database connection string
string connectionString = builder.Configuration.GetConnectionString("Conn") ?? "";

// Get current environment from configuration
var currentDB = builder.Configuration["currentDB"];
Console.WriteLine($"Current DB from Config: {currentDB}");
bool isSQL = currentDB == "SQL" ? true : false;

//Inject MySQL password into connection string if it's missing
if (!string.IsNullOrWhiteSpace(mysqlPassword) && connectionString.Contains("__MYSQL_ROOT_PASSWORD__"))
{
    connectionString = connectionString.Replace("__MYSQL_ROOT_PASSWORD__", mysqlPassword);
}

Console.WriteLine($"ConnectionString: {connectionString}");

// Add services to the container.
if (isSQL)
{
    Console.WriteLine($"Running in {env.EnvironmentName} mode (Using MySQL)");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}
else
{
    Console.WriteLine($"Running in {env.EnvironmentName} mode (Using In-Memory Database)");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<ILoanApplicationRepo, LoanApplicationRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IChecklistRepo, ChecklistRepo>();
builder.Services.AddScoped<IEsgAiRecommendationRepo, EsgAiRecommendationRepo>();
builder.Services.AddScoped<IEsgAiRecommendationService, EsgAiRecommendationService>();
builder.Services.AddScoped<IEsgExplainabilityService, EsgExplainabilityService>();
builder.Services.AddScoped<IEsgMlFeatureService, EsgMlFeatureService>();
builder.Services.AddScoped<IEsgMlSignalService, EsgMlSignalService>();
builder.Services.AddScoped<IAuthRepo, AuthRepo>();

builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter JWT token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    // Only apply auth to [Authorize] endpoints
    options.OperationFilter<AuthorizeCheckOperationFilter>();
});

builder.Services.AddSwaggerGen();
builder.Services.AddApiRateLimiting();

// JWT Authentication configuration
var jwtSettings = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSettings.GetValue<string>("Key");
if (string.IsNullOrEmpty(jwtKey))
    throw new InvalidOperationException("Jwt:Key is not configured. Add it in appsettings.json.");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
        ValidAudience = jwtSettings.GetValue<string>("Audience"),
        IssuerSigningKey = key
    };
});

var app = builder.Build();
PrepDb.PrepPopulation(app, isSQL);
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseCors();
app.UseRateLimiter();
app.MapControllers();
//app.UseHttpsRedirection();

app.Run();