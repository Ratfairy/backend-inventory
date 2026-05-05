using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.PurchaseOrder;

public class UpdatePODto
{
    [Required]
    [MaxLength(100)]
    public string Supplier { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<CreatePOItemDto> Items { get; set; } = new();
}