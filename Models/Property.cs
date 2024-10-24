using System.ComponentModel.DataAnnotations;
namespace PropertyApp.Models;
public class Property
{
    public int Id { get; set; }

    [Required]
    public required string Title { get; set; }

    [Required]
    public required string Address { get; set; }

    [Required]
    [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive number.")]
    public decimal Price { get; set; }

    [Required]
    public int Bedrooms { get; set; }

    [Required]
    public int Bathrooms { get; set; }
}
