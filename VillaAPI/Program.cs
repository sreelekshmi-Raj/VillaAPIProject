using Microsoft.EntityFrameworkCore;
using Serilog;
using VillaAPI;
using VillaAPI.Data;
using VillaAPI.Logging;
using VillaAPI.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//custom implementation of log informnation - configuring the logger configuration using Siri log
//Log.Logger = new LoggerConfiguration().MinimumLevel.Debug()
//    .WriteTo.File("log/villaLogs.txt", rollingInterval: RollingInterval.Day).CreateLogger();

//rather than using Built in console logging use serilog
//builder.Host.UseSerilog();

//Add database connection
builder.Services.AddDbContext<ApplicationDbContext>(option =>
{
    option.UseSqlServer(builder.Configuration.GetConnectionString("SQLConnection"));
}
);
//Inject Repository
builder.Services.AddScoped<IVillaRepository, VillaRepository>();
builder.Services.AddScoped<IVillaNumberRepository, VillaNumberRepository>();
//register mapping 
builder.Services.AddAutoMapper(typeof(MappingConfig));
//builder.Services.AddAutoMapper(typeof(Program).Assembly);

builder.Services.AddControllers(option =>
{
   // option.ReturnHttpNotAcceptable = true;
}).AddNewtonsoftJson().AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//builder.Services.AddSingleton<ILogging, LoggingV2>();

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
