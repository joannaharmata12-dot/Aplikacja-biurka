using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;

namespace DeskBooking.Application.Services;

public class DeskBookingService
{
    private readonly IUnitOfWork _unitOfWork;

    public DeskBookingService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Floor>> GetFloorsAsync()
    {
        return await _unitOfWork.Repository<Floor>().GetAllAsync();
    }

    public async Task<IEnumerable<Desk>> GetDesksByFloorAsync(int floorId)
    {
        var allDesks = await _unitOfWork.Repository<Desk>().GetAllAsync();
        return allDesks.Where(d => d.FloorId == floorId);
    }

    public async Task<IEnumerable<Room>> GetRoomsByFloorAsync(int floorId)
    {
        var allRooms = await _unitOfWork.Repository<Room>().GetAllAsync();
        return allRooms.Where(r => r.FloorId == floorId);
    }

    public async Task<IEnumerable<PhoneBooth>> GetPhoneBoothsByFloorAsync(int floorId)
    {
        var allBooths = await _unitOfWork.Repository<PhoneBooth>().GetAllAsync();
        return allBooths.Where(b => b.FloorId == floorId);
    }

    public async Task<bool> IsDeskAvailableAsync(int deskId, string date, TimeSpan timeFrom, TimeSpan timeTo)
    {
        if (timeTo <= timeFrom)
            return false;

        var allBookings = await _unitOfWork.Repository<Booking>().GetAllAsync();

        var isOccupied = allBookings.Any(b =>
            b.DeskId == deskId &&
            b.BookingDate == date &&
            b.Status == "Confirmed" &&
            timeFrom < b.TimeTo &&
            timeTo > b.TimeFrom
        );

        return !isOccupied;
    }

    public async Task<bool> BookDeskAsync(
        int deskId,
        int userId,
        string employeeName,
        string employeeEmail,
        string note,
        string date,
        TimeSpan timeFrom,
        TimeSpan timeTo)
    {
        var isAvailable = await IsDeskAvailableAsync(deskId, date, timeFrom, timeTo);

        if (!isAvailable)
            return false;

        var desks = await _unitOfWork.Repository<Desk>().GetAllAsync();
        var desk = desks.FirstOrDefault(d => d.Id == deskId);

        var booking = new Booking
        {
            DeskId = deskId,
            UserId = userId,

            ResourceType = "Desk",
            ResourceId = deskId,
            ResourceName = desk?.DeskNumber ?? $"Biurko {deskId}",

            EmployeeName = employeeName,
            EmployeeEmail = employeeEmail,
            Note = note,
            BookingDate = date,
            TimeFrom = timeFrom,
            TimeTo = timeTo,
            Status = "Confirmed"
        };

        await _unitOfWork.Repository<Booking>().AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    public async Task<IEnumerable<BookingHistory>> GetBookingHistoryAsync()
    {
        return await _unitOfWork.Repository<BookingHistory>().GetAllAsync();
    }

    public async Task<IEnumerable<Booking>> GetActiveBookingsAsync()
    {
        return await _unitOfWork.Repository<Booking>().GetAllAsync();
    }

    public async Task<IEnumerable<Booking>> GetBookingsForDeskAsync(int deskId, string date)
    {
        var allBookings = await _unitOfWork.Repository<Booking>().GetAllAsync();

        return allBookings
            .Where(b =>
                b.DeskId == deskId &&
                b.BookingDate == date &&
                b.Status == "Confirmed")
            .OrderBy(b => b.TimeFrom);
    }
    public async Task<bool> CancelBookingAsync(int bookingId)
    {
        var bookings = await _unitOfWork.Repository<Booking>().GetAllAsync();
        var booking = bookings.FirstOrDefault(b => b.Id == bookingId);

        if (booking == null)
            return false;

        booking.Status = "Cancelled";

        await _unitOfWork.SaveChangesAsync();

        return true;
    }
    public async Task<IEnumerable<Booking>> GetBookingsByUserAsync(int userId)
    {
        var allBookings = await _unitOfWork.Repository<Booking>().GetAllAsync();

        return allBookings
            .Where(b => b.UserId == userId)
            .OrderBy(b => b.BookingDate)
            .ThenBy(b => b.TimeFrom);
    }
    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var users = await _unitOfWork.Repository<User>().GetAllAsync();

        return users.FirstOrDefault(u => u.Id == userId);
    }
    public async Task<bool> IsResourceAvailableAsync(string resourceType, int resourceId, string date, TimeSpan timeFrom, TimeSpan timeTo)
    {
        if (timeTo <= timeFrom)
            return false;

        var allBookings = await _unitOfWork.Repository<Booking>().GetAllAsync();

        var isOccupied = allBookings.Any(b =>
            b.ResourceType == resourceType &&
            b.ResourceId == resourceId &&
            b.BookingDate == date &&
            b.Status == "Confirmed" &&
            timeFrom < b.TimeTo &&
            timeTo > b.TimeFrom
        );

        return !isOccupied;
    }

    public async Task<bool> BookResourceAsync(
        string resourceType,
        int resourceId,
        string resourceName,
        int userId,
        string employeeName,
        string employeeEmail,
        string note,
        string date,
        TimeSpan timeFrom,
        TimeSpan timeTo)
    {
        var isAvailable = await IsResourceAvailableAsync(resourceType, resourceId, date, timeFrom, timeTo);

        if (!isAvailable)
            return false;

        var booking = new Booking
        {
            DeskId = resourceType == "Desk" ? resourceId : 1001,
            UserId = userId,

            ResourceType = resourceType,
            ResourceId = resourceId,
            ResourceName = resourceName,

            EmployeeName = employeeName,
            EmployeeEmail = employeeEmail,
            Note = note,
            BookingDate = date,
            TimeFrom = timeFrom,
            TimeTo = timeTo,
            Status = "Confirmed"
        };

        await _unitOfWork.Repository<Booking>().AddAsync(booking);
        await _unitOfWork.SaveChangesAsync();

        return true;
    }
}

