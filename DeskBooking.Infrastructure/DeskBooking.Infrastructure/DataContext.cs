using DeskBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DeskBooking.Infrastructure;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Floor> Floors => Set<Floor>();
    public DbSet<Desk> Desks => Set<Desk>();
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<PhoneBooth> PhoneBooths => Set<PhoneBooth>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<BookingHistory> BookingHistories => Set<BookingHistory>();
    public DbSet<Building> Buildings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);


        // BUILDING
        modelBuilder.Entity<Building>().HasData(
            new Building { Id = 1, Name = "Budynek A" }
        );


        // FLOORS
        modelBuilder.Entity<Floor>().HasData(
            new Floor { Id = 1, Name = "Parter", BuildingId = 1, FloorNumber = 0, LayoutImagePath = "images/maps/plan.png" },
            new Floor { Id = 2, Name = "1 Piętro", BuildingId = 1, FloorNumber = 1, LayoutImagePath = "images/maps/plan.png" },
            new Floor { Id = 3, Name = "2 Piętro", BuildingId = 1, FloorNumber = 2, LayoutImagePath = "images/maps/plan.png" }
        );

        // DESKS — TEMPLATE (36 szt.)
        var desksFloor1 = new List<Desk>
        {
            new Desk { Id = 1001, DeskNumber = "A1", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 19 },
            new Desk { Id = 1002, DeskNumber = "A2", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 19 },
            new Desk { Id = 1003, DeskNumber = "A3", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 25 },
            new Desk { Id = 1004, DeskNumber = "A4", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 25 },
            new Desk { Id = 1005, DeskNumber = "A5", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 32 },
            new Desk { Id = 1006, DeskNumber = "A6", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 32 },
            new Desk { Id = 1007, DeskNumber = "A7", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 39 },
            new Desk { Id = 1008, DeskNumber = "A8", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 39 },

            new Desk { Id = 1009, DeskNumber = "B1", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 44 },
            new Desk { Id = 1010, DeskNumber = "B2", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 44 },
            new Desk { Id = 1011, DeskNumber = "B3", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 52 },
            new Desk { Id = 1012, DeskNumber = "B4", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 52 },
            new Desk { Id = 1013, DeskNumber = "B5", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 58 },
            new Desk { Id = 1014, DeskNumber = "B6", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 58 },
            new Desk { Id = 1015, DeskNumber = "B7", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 64 },
            new Desk { Id = 1016, DeskNumber = "B8", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 64 },

            new Desk { Id = 1017, DeskNumber = "C1", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 70 },
            new Desk { Id = 1018, DeskNumber = "C2", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 70 },
            new Desk { Id = 1019, DeskNumber = "C3", FloorId = 1, X_Coordinate = 12, Y_Coordinate = 77 },
            new Desk { Id = 1020, DeskNumber = "C4", FloorId = 1, X_Coordinate = 28, Y_Coordinate = 77 },
            new Desk { Id = 1021, DeskNumber = "C5", FloorId = 1, X_Coordinate = 72, Y_Coordinate = 59 },
            new Desk { Id = 1022, DeskNumber = "C6", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 59 },
            new Desk { Id = 1023, DeskNumber = "C7", FloorId = 1, X_Coordinate = 72, Y_Coordinate = 66 },
            new Desk { Id = 1024, DeskNumber = "C8", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 66 },

            new Desk { Id = 1025, DeskNumber = "D1", FloorId = 1, X_Coordinate = 71, Y_Coordinate = 18 },
            new Desk { Id = 1026, DeskNumber = "D2", FloorId = 1, X_Coordinate = 71, Y_Coordinate = 24 },
            new Desk { Id = 1027, DeskNumber = "D3", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 15 },
            new Desk { Id = 1028, DeskNumber = "D4", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 22 },

            new Desk { Id = 1029, DeskNumber = "D5", FloorId = 1, X_Coordinate = 72, Y_Coordinate = 33 },
            new Desk { Id = 1030, DeskNumber = "D6", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 33 },
            new Desk { Id = 1031, DeskNumber = "D7", FloorId = 1, X_Coordinate = 72, Y_Coordinate = 41 },
            new Desk { Id = 1032, DeskNumber = "D8", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 41 },

            new Desk { Id = 1033, DeskNumber = "D9", FloorId = 1, X_Coordinate = 73, Y_Coordinate = 47 },
            new Desk { Id = 1034, DeskNumber = "D10", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 47 },
            new Desk { Id = 1035, DeskNumber = "D11", FloorId = 1, X_Coordinate = 73, Y_Coordinate = 54 },
            new Desk { Id = 1036, DeskNumber = "D12", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 54 }
        };

        // FLOOR 1
        modelBuilder.Entity<Desk>().HasData(desksFloor1);

        // FLOOR 2 — SAME COORDS, DIFFERENT IDs
        modelBuilder.Entity<Desk>().HasData(
            desksFloor1.Select(d => new Desk
            {
                Id = d.Id + 1000,
                DeskNumber = d.DeskNumber,
                FloorId = 2,
                X_Coordinate = d.X_Coordinate,
                Y_Coordinate = d.Y_Coordinate
            })
        );

        // FLOOR 3 — SAME COORDS, DIFFERENT IDs
        modelBuilder.Entity<Desk>().HasData(
            desksFloor1.Select(d => new Desk
            {
                Id = d.Id + 2000,
                DeskNumber = d.DeskNumber,
                FloorId = 3,
                X_Coordinate = d.X_Coordinate,
                Y_Coordinate = d.Y_Coordinate
            })
        );

        // ROOMS
        modelBuilder.Entity<Room>().HasData(
    new Room { Id = 2001, Name = "Conference Room 0.1", FloorId = 1, X_Coordinate = 80, Y_Coordinate = 80 },
    new Room { Id = 2101, Name = "Conference Room 1.1", FloorId = 2, X_Coordinate = 80, Y_Coordinate = 80 },
    new Room { Id = 2201, Name = "Conference Room 2.1", FloorId = 3, X_Coordinate = 80, Y_Coordinate = 80 }
        );

        // PHONE BOOTHS
        modelBuilder.Entity<PhoneBooth>().HasData(
    new PhoneBooth { Id = 3001, Name = "Phone Booth 0.1", FloorId = 1, X_Coordinate = 71, Y_Coordinate = 94 },
    new PhoneBooth { Id = 3002, Name = "Phone Booth 0.2", FloorId = 1, X_Coordinate = 88, Y_Coordinate = 94 },

    new PhoneBooth { Id = 3101, Name = "Phone Booth 1.1", FloorId = 2, X_Coordinate = 71, Y_Coordinate = 94 },
    new PhoneBooth { Id = 3102, Name = "Phone Booth 1.2", FloorId = 2, X_Coordinate = 88, Y_Coordinate = 94 },

    new PhoneBooth { Id = 3201, Name = "Phone Booth 2.1", FloorId = 3, X_Coordinate = 71, Y_Coordinate = 94 },
    new PhoneBooth { Id = 3202, Name = "Phone Booth 2.2", FloorId = 3, X_Coordinate = 88, Y_Coordinate = 94 }
);
    }
}
