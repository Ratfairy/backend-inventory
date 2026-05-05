using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.PurchaseOrder;

public class CreatePODto
{
    [Required]
    public int PurchaseRequestId { get; set; }

    [Required]
    [MaxLength(100)]
    public string Supplier { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<CreatePOItemDto> Items { get; set; } = new();
}

public class CreatePOItemDto
{
    [Required]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int Qty { get; set; }

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }

    [MaxLength(255)]
    public string? Reason { get; set; }
}