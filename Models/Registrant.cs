// Models/Registrant.cs
using System.ComponentModel.DataAnnotations;

namespace PRS.Models;

public class Registrant
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [MaxLength(100)]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [MaxLength(100)]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Birthday is required.")]
    public DateOnly Birthday { get; set; }

    [Required(ErrorMessage = "Gender is required.")]
    public string Gender { get; set; } = string.Empty;

    [Required(ErrorMessage = "Contact number is required.")]
    [Phone(ErrorMessage = "Invalid contact number.")]
    public string ContactNo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress(ErrorMessage = "Invalid email address.")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Address is required.")]   
    public string Address { get; set; } = string.Empty;

    public string? Signature { get; set; }  // Base64 PNG

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public int? EventId { get; set; }
    public Event? Event { get; set; }
}
