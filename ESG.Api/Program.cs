using System.Threading.RateLimiting;
using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Repository;
using ESG.Api.Services;
using Microsoft.EntityFrameworkCore;

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
builder.Services.AddSwaggerGen();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, _) =>
    {
        var retryAfter = context.Lease.TryGetMetadata(
            MetadataName.RetryAfter,
            out var retryAfterTime)
            ? retryAfterTime
            : TimeSpan.FromSeconds(60);

        var retryAtUtc = DateTimeOffset.UtcNow.Add(retryAfter);

        context.HttpContext.Response.Headers["Retry-After"] =
            retryAtUtc.ToString("R"); // RFC 1123 format

        await context.HttpContext.Response.WriteAsync(
            $"Too many requests. Retry at {retryAtUtc:HH:mm:ss} UTC"
        );
    };    
    
    options.AddPolicy("GetChecklists", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetClientKey(context),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("GetAllLoanApplications", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetClientKey(context),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 30,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("AssessmentSubmit", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetClientKey(context),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("LoanCreate", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetClientKey(context),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 3,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("AiRecommendation", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            GetClientKey(context),
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 2,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));
});

string GetClientKey(HttpContext context)
{
    return context.User.Identity?.IsAuthenticated == true
        ? context.User.Identity!.Name!
        : context.Connection.RemoteIpAddress?.ToString() ?? "anonymous";
}

var app = builder.Build();
PrepDb.PrepPopulation(app, isSQL);
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.UseRateLimiter();
app.MapControllers();
app.UseCors();
//app.UseHttpsRedirection();

app.Run();