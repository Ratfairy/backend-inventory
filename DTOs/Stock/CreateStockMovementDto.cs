using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Stock;

public class CreateStockMovementDto
{
    [Required]
    public int StockId { get; set; }

    [Required]
    [RegularExpression("^(IN|OUT)$", ErrorMessage = "Type harus IN atau OUT")]
    public string Type { get; set; } = string.Empty;

    [Required]
    [Range(1, int.MaxValue)]
    public int Qty { get; set; }

    [MaxLength(255)]
    public string? Description { get; set; }

    [MaxLength(100)]
    public string? Pic { get; set; }
}