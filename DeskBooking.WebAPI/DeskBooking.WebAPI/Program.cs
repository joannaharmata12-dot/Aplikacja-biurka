using DeskBooking.Application.Services;
using DeskBooking.Domain.Interfaces;
using DeskBooking.Infrastructure;
using DeskBooking.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using DeskBooking.Application.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args); 

// Konfiguracja Seriloga na podstawie appsettings.json
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Konfiguracja bazy danych SQLite
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(connectionString));

// Rejestracja repozytoriów i serwisów w kontenerze DI
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DeskBookingService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// W³¹czenie logowania ¿¹dañ HTTP
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();

try
{
    Log.Information("Uruchamianie aplikacji WebAPI...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Aplikacja WebAPI nie mog³a wystartowaæ poprawnie.");
}
finally
{
    Log.CloseAndFlush();
}