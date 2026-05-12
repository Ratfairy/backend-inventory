using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Adjustment;

public class CreateAdjustmentDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(100)]
    public string Pic { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Reason { get; set; }

    [Required]
    [MinLength(1)]
    public List<CreateAdjustmentItemDto> Items { get; set; }
        = new();
}

public class CreateAdjustmentItemDto
{
    [Required]
    public int StockId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int ActualQty { get; set; }
}