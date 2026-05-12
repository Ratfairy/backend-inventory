using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Stock;

public class CreateStockMovementDto
{
    [Required]
    public int StockId { get; set; }

    [Required]
    public string Type { get; set; } = string.Empty;

    [Required]
    public int Qty { get; set; }

    public string? Description { get; set; }

    public string? Pic { get; set; }

    public string? ReferenceType { get; set; }
}