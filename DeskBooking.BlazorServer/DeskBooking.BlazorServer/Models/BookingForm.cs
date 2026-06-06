using System;
using System.ComponentModel.DataAnnotations;

namespace DeskBooking.BlazorServer.Models;

public class BookingForm
{
    [Required(ErrorMessage = "Imię i nazwisko pracownika jest wymagane.")]
    [StringLength(50, ErrorMessage = "Imię i nazwisko jest za długie.")]
    public string EmployeeName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Adres email jest wymagany.")]
    [EmailAddress(ErrorMessage = "Wprowadź poprawny adres email.")]
    public string EmployeeEmail { get; set; } = string.Empty;

    [Required(ErrorMessage = "Godzina rozpoczęcia jest wymagana.")]
    public TimeSpan? StartTime { get; set; } = new TimeSpan(8, 0, 0);

    [Required(ErrorMessage = "Godzina zakończenia jest wymagana.")]
    public TimeSpan? EndTime { get; set; } = new TimeSpan(16, 0, 0);

    [Required(ErrorMessage = "Musisz podać powód rezerwacji.")]
    [MinLength(5, ErrorMessage = "Opis musi mieć co najmniej 5 znaków.")]
    public string Note { get; set; } = string.Empty;
}