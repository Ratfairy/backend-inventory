using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Stock;

public class CreateStockDto
{
    [Required]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Required]
    [Range(0, int.MaxValue)]
    public int Qty { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int MinQty { get; set; }

    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Price { get; set; }
}