using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.StockOpname;

public class CreateStockOpnameDto
{
    [Required]
    public DateTime Date { get; set; }

    [Required]
    [MaxLength(100)]
    public string Pic { get; set; } = string.Empty;

    [Required]
    [MinLength(1)]
    public List<CreateStockOpnameItemDto> Items { get; set; } = new();
}

public class CreateStockOpnameItemDto
{
    [Required]
    public int StockId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int ActualQty { get; set; }
}