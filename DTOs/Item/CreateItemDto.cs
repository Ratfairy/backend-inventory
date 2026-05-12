using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Item;

public class CreateItemDto
{
    [Required]
    public int CategoryId { get; set; }

    [Required]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [MaxLength(100)]
    public string? CreatedBy { get; set; }
}