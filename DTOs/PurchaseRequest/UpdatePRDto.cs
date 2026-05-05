using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.PurchaseRequest;

public class UpdatePRDto
{
    [Required]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string Pic { get; set; } = string.Empty;

    [Required]
    public DateTime NeededDate { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreatePRItemDto> Items { get; set; } = new();
}