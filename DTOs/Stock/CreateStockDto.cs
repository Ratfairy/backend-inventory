using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Stock;

public class CreateStockDto
{
    [Required]
    public int ItemId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int Qty { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int MinQty { get; set; }
}
