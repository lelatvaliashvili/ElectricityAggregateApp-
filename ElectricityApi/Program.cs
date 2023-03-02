using AutoMapper;
using Electricity.Application.Services;
using Electricity.Domain.Interfaces;
using Electricity.Domain.IService;
using Electricity.Infrastructure.Data;
using Electricity.Infrastructure.Profiles;
using Electricity.Infrastructure.Services;
using ElectricityApi.Middleware;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);


// Load configuration from appsettings.json
var configuration = builder.Configuration;

var logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .Enrich.FromLogContext()
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Get the connection string from appsettings.json
var connectionString = configuration.GetConnectionString("DefaultConnection");

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Public Electricity Data API", Version = "v1" });
});

//register services
builder.Services.AddSingleton<HttpClient>();

// Register services and repositories
builder.Services.AddScoped<ICsvDataService, CsvDataService>();
builder.Services.AddScoped<IElectricityConsumptionService, ElectricityConsumptionService>();
builder.Services.AddScoped<IElectricityConsumptionRepository, ElectricityConsumptionRepository>();
builder.Services.AddAutoMapper(typeof(Program).Assembly);
builder.Services.AddDbContext<ElectricityDbContext>(options => options.UseSqlServer(connectionString).EnableSensitiveDataLogging());

var mappingConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mappingConfig.CreateMapper();
builder.Services.AddSingleton(mapper);


var app = builder.Build();

app.UseMiddleware<ExceptionHandlingMiddleware>();

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
