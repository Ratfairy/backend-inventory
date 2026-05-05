namespace backend_inventory.DTOs.Stock;

public class StockResponseDto
{
    public int Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public int MinQty { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Status => Qty < MinQty ? "Low Stock" : "Available";
    public DateTime UpdatedAt { get; set; }
    public DateTime CreatedAt { get; set; }
}