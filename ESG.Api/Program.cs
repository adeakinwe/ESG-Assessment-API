using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Repository;
using ESG.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
Console.WriteLine($"Current Environment: {env.EnvironmentName}");

// Get MySQL password from environment variables (SECURE)
string mysqlPassword = Environment.GetEnvironmentVariable("MYSQL_ROOT_PASSWORD") ?? "mysqluser10$";
//string mysqlPassword = builder.Configuration["MYSQL_ROOT_PASSWORD"];
Console.WriteLine($"Password: {mysqlPassword}");
if (string.IsNullOrWhiteSpace(mysqlPassword))
{
    Console.WriteLine("Warning: MYSQL_ROOT_PASSWORD is not set. Ensure it is configured correctly.");
}

// Get database connection string
string connectionString = builder.Configuration.GetConnectionString("Conn") ?? "";

// Get current environment from configuration
var currentEnv =  builder.Configuration["CurrentEnvironment"];
Console.WriteLine($"Current Environment from Config: {currentEnv}");
bool isProdEnv = currentEnv?.ToLower() == "production" ? true : false;

//Inject MySQL password into connection string if it's missing
if (!string.IsNullOrWhiteSpace(mysqlPassword) && connectionString.Contains("__MYSQL_ROOT_PASSWORD__"))
{
    connectionString = connectionString.Replace("__MYSQL_ROOT_PASSWORD__", mysqlPassword);
}

Console.WriteLine($"ConnectionString: {connectionString}");

// Add services to the container.
if (isProdEnv)
{
    Console.WriteLine("Running in Production mode (Using MySQL)");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}
else
{
    Console.WriteLine("Running in Development mode (Using In-Memory Database)");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<ILoanApplicationRepo, LoanApplicationRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
builder.Services.AddScoped<IChecklistRepo, ChecklistRepo>();
builder.Services.AddScoped<IEsgAiRecommendationRepo, EsgAiRecommendationRepo>();
builder.Services.AddScoped<IEsgAiRecommendationService, EsgAiRecommendationService>();
builder.Services.AddScoped<IEsgExplainabilityService, EsgExplainabilityService>();

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

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
PrepDb.PrepPopulation(app, isProdEnv);
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
app.UseCors();
//app.UseHttpsRedirection();

app.Run();