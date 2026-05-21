// Models/Event.cs
using System.ComponentModel.DataAnnotations;
namespace PRS.Models;

public class Event
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Activity name is required.")]
    [MaxLength(200)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Description is required.")]
    public string Description { get; set; } = string.Empty;

    [Required(ErrorMessage = "Start date is required.")]
    public DateOnly StartDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "End date is required.")]
    public DateOnly EndDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);

    [Required(ErrorMessage = "Location is required.")]
    [MaxLength(200)]
    public string Location { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int CreatedBy { get; set; }  
    public List<Registrant> Registrants { get; set; } = new();

    public bool IsUpcoming => StartDate > DateOnly.FromDateTime(DateTime.Today);
    public bool IsOngoing  => StartDate <= DateOnly.FromDateTime(DateTime.Today) && EndDate >= DateOnly.FromDateTime(DateTime.Today);
    public bool IsPast     => EndDate < DateOnly.FromDateTime(DateTime.Today);

    public string StatusLabel => IsOngoing ? "Ongoing" : IsUpcoming ? "Upcoming" : "Completed";
    public string StatusClass => IsOngoing ? "status-ongoing" : IsUpcoming ? "status-upcoming" : "status-completed";
}