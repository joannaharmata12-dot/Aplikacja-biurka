namespace DeskBooking.SharedKernel;

public class BookingDto
{
    public int Id { get; set; }
    public int DeskId { get; set; }
    public int UserId { get; set; }
    public TimeSpan TimeFrom { get; set; }
    public TimeSpan TimeTo { get; set; }
    public string BookingDate { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}