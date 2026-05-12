using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Stock;

public class UpdateStockDto
{
    [Required]
    public int Qty { get; set; }

    [Required]
    public int MinQty { get; set; }
}