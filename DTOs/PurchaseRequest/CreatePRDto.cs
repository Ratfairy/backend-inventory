using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.PurchaseRequest;

public class CreatePRDto
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

public class CreatePRItemDto
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