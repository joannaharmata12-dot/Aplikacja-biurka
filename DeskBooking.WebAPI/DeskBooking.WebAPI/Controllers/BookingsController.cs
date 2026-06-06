using DeskBooking.Application.Services;
using DeskBooking.Domain.Entities;
using DeskBooking.Domain.Interfaces;
using DeskBooking.SharedKernel;
using Microsoft.AspNetCore.Mvc;
using DeskBooking.Application.Services; 
namespace DeskBooking.WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BookingsController : ControllerBase
{
    private readonly DeskBookingService _bookingService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<BookingsController> _logger;

    public BookingsController(DeskBookingService bookingService, IUnitOfWork unitOfWork, ILogger<BookingsController> logger)
    {
        _bookingService = bookingService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    // GET: api/bookings (Wszystkie aktywne rezerwacje)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Booking>>> GetBookings()
    {
        _logger.LogInformation("Pobieranie listy wszystkich aktywnych rezerwacji.");
        var bookings = await _bookingService.GetActiveBookingsAsync();
        return Ok(bookings); 
    }

    // POST: api/bookings (Tworzenie nowej rezerwacji z walidacją i logowaniem błędu)
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingDto dto)
    {
        _logger.LogInformation("Próba utworzenia rezerwacji dla biurka {DeskId} przez użytkownika {UserId}.", dto.DeskId, dto.UserId);

        if (dto.DeskId <= 0 || dto.UserId <= 0 || string.IsNullOrEmpty(dto.BookingDate))
        {
            _logger.LogWarning("Niepoprawne dane w formularzu rezerwacji.");
            return BadRequest("Dane formularza są niekompletne.");
        }

        var result = await _bookingService.BookDeskAsync(
            dto.DeskId,
            dto.UserId,
            string.Empty,
            string.Empty,
            string.Empty,
            dto.BookingDate,
            dto.TimeFrom,
            dto.TimeTo);

        if (!result)
        {
            _logger.LogError("BŁĄD: Biurko {DeskId} jest już zajęte w dniu {Date}!", dto.DeskId, dto.BookingDate);
            return BadRequest("To biurko jest już zarezerwowane w wybranym dniu.");
        }

        return Ok(new { Message = "Rezerwacja zakończona sukcesem." });
    }

    // DELETE: api/bookings/5 (Usuwanie rezerwacji - CRUD)
    [HttpDelete("{id}")]
    public async Task<IActionResult> CancelBooking(int id)
    {
        _logger.LogInformation("Próba anulowania rezerwacji o ID: {Id}", id);

        var repo = _unitOfWork.Repository<Booking>();
        var booking = await repo.GetByIdAsync(id);

        if (booking == null)
        {
            _logger.LogWarning("Nie znaleziono rezerwacji o ID: {Id} do anulowania.", id);
            return NotFound();
        }

        repo.Delete(booking);
        await _unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Pomyślnie anulowano rezerwację o ID: {Id}", id);
        return NoContent();
    }
}