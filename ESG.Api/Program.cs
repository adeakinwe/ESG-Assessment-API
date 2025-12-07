using ESG.Api.Data;
using ESG.Api.Interface;
using ESG.Api.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var env = builder.Environment;
Console.WriteLine($"Current Environment: {env.EnvironmentName}");

// Add services to the container.
if (env.IsProduction())
{
    Console.WriteLine("Running in Production mode (Using MySQL)");
    // builder.Services.AddDbContext<AppDbContext>(options =>
    //     options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));
}
else
{
    Console.WriteLine("Running in Development mode (Using In-Memory Database)");
    builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseInMemoryDatabase("InMem"));
}
builder.Services.AddScoped<ILoanApplicationRepo, LoanApplicationRepo>();
builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
PrepDb.PrepPopulation(app, env.IsProduction());
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseAuthorization();
app.MapControllers();
//app.UseHttpsRedirection();

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
