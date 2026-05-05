using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.StockOpname;

public class UpdateStockOpnameStatusDto
{
    [Required]
    [RegularExpression("^(APPROVED|REJECTED)$",
        ErrorMessage = "Status harus APPROVED atau REJECTED")]
    public string Status { get; set; } = string.Empty;
}