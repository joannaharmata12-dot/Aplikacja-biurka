using DeskBooking.Domain.Interfaces;
using DeskBooking.Infrastructure.Repositories;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Dodaj us³ugi do kontenera DI
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddMudServices();

// Rejestracja bazy danych SQLite // pobiera konfiguracjê z appsettings.json
builder.Services.AddDbContext<DeskBooking.Infrastructure.DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=Nowa_Baza_Biurek.db"));

// REJESTRACJA US£UG (usuwa b³¹d CS0234)

builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DeskBooking.Application.Services.DeskBookingService>();

var app = builder.Build();


// SEKCJA WYMUSZENIA TWORZENIA NOWEJ BAZY, U¯YTKOWNIKA I BIUREK

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DeskBooking.Infrastructure.DataContext>();

        // fizyczne utworzenie bazy i tabel
        context.Database.EnsureCreated();

        // U¯YTKOWNIK TESTOWY zapobiega b³êdom 
        
        if (!context.Set<DeskBooking.Domain.Entities.User>().Any())
        {
            var testUser = new DeskBooking.Domain.Entities.User
            {
                Id = 1,
                FirstName = "Jan",
                LastName = "Kowalski",
                Email = "jan.kowalski@firma.pl"
            };
            context.Set<DeskBooking.Domain.Entities.User>().Add(testUser);
            context.SaveChanges(); // Trwale zapisuje u¿ytkownika z Id = 1
        }

        // PIÊTRO
        if (!context.Set<DeskBooking.Domain.Entities.Floor>().Any())
        {
            var testFloor = new DeskBooking.Domain.Entities.Floor
            {
                Name = "Parter - Strefa A",
                BuildingName = "Budynek G³ówny"
            };
            context.Set<DeskBooking.Domain.Entities.Floor>().Add(testFloor);
            context.SaveChanges();

            // BIURKA (Wszystkie 30 sztuk z liczbami ca³kowitymi)
            if (!context.Set<DeskBooking.Domain.Entities.Desk>().Any())
            {
                context.Set<DeskBooking.Domain.Entities.Desk>().AddRange(
                    // LEWA STRONA: OPERATIONS AREA (5 BOKSÓW)
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L1-1", X_Coordinate = 18, Y_Coordinate = 19, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L1-2", X_Coordinate = 33, Y_Coordinate = 19, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L1-3", X_Coordinate = 18, Y_Coordinate = 24, IsAvailable = false, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L1-4", X_Coordinate = 33, Y_Coordinate = 24, IsAvailable = true, FloorId = testFloor.Id },

                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L2-1", X_Coordinate = 18, Y_Coordinate = 33, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L2-2", X_Coordinate = 33, Y_Coordinate = 33, IsAvailable = false, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L2-3", X_Coordinate = 18, Y_Coordinate = 37, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L2-4", X_Coordinate = 33, Y_Coordinate = 37, IsAvailable = true, FloorId = testFloor.Id },

                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L3-1", X_Coordinate = 18, Y_Coordinate = 45, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L3-2", X_Coordinate = 33, Y_Coordinate = 45, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L3-3", X_Coordinate = 18, Y_Coordinate = 49, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L3-4", X_Coordinate = 33, Y_Coordinate = 49, IsAvailable = false, FloorId = testFloor.Id },

                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L4-1", X_Coordinate = 18, Y_Coordinate = 58, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L4-2", X_Coordinate = 33, Y_Coordinate = 58, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L4-3", X_Coordinate = 18, Y_Coordinate = 62, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L4-4", X_Coordinate = 33, Y_Coordinate = 62, IsAvailable = true, FloorId = testFloor.Id },

                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L5-1", X_Coordinate = 18, Y_Coordinate = 71, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L5-2", X_Coordinate = 33, Y_Coordinate = 71, IsAvailable = false, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L5-3", X_Coordinate = 18, Y_Coordinate = 75, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "L5-4", X_Coordinate = 33, Y_Coordinate = 75, IsAvailable = true, FloorId = testFloor.Id },

                    // PRAWA STRONA: CREATIVE STUDIO (STO£Y PIONOWE)
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "CS-1", X_Coordinate = 59, Y_Coordinate = 19, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "CS-2", X_Coordinate = 59, Y_Coordinate = 24, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "CS-3", X_Coordinate = 89, Y_Coordinate = 15, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "CS-4", X_Coordinate = 89, Y_Coordinate = 22, IsAvailable = false, FloorId = testFloor.Id },

                    // PRAWA STRONA: OPERATIONS AREA (3 BOKSY PO PRAWEJ)
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R1-1", X_Coordinate = 73, Y_Coordinate = 35, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R1-2", X_Coordinate = 88, Y_Coordinate = 35, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R1-3", X_Coordinate = 73, Y_Coordinate = 40, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R1-4", X_Coordinate = 88, Y_Coordinate = 40, IsAvailable = true, FloorId = testFloor.Id },

                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R2-1", X_Coordinate = 73, Y_Coordinate = 48, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R2-2", X_Coordinate = 88, Y_Coordinate = 48, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R2-3", X_Coordinate = 73, Y_Coordinate = 52, IsAvailable = false, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R2-4", X_Coordinate = 88, Y_Coordinate = 52, IsAvailable = true, FloorId = testFloor.Id },

                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R3-1", X_Coordinate = 73, Y_Coordinate = 61, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R3-2", X_Coordinate = 88, Y_Coordinate = 61, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R3-3", X_Coordinate = 73, Y_Coordinate = 65, IsAvailable = true, FloorId = testFloor.Id },
                    new DeskBooking.Domain.Entities.Desk { DeskNumber = "R3-4", X_Coordinate = 88, Y_Coordinate = 65, IsAvailable = false, FloorId = testFloor.Id }
                );
                context.SaveChanges();
            }
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"B³¹d podczas generowania nowej bazy: {ex.Message}");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();