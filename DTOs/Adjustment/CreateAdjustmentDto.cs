using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Adjustment;

public class CreateAdjustmentDto
{
    [Required]
    public int StockId { get; set; }

    [Required]
    public int AdjustmentQty { get; set; }

    [MaxLength(255)]
    public string? Reason { get; set; }

    [MaxLength(100)]
    public string? Pic { get; set; }
}