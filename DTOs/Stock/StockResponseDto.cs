namespace backend_inventory.DTOs.Stock;

public class StockResponseDto
{
    public int Id { get; set; }

    public int? ItemId { get; set; }

    public int Qty { get; set; }

    public int MinQty { get; set; }

    public string Status =>
        Qty < MinQty
            ? "Low Stock"
            : "Available";

    // ITEM
    public string ItemName { get; set; }
        = string.Empty;

    public string CategoryName { get; set; }
        = string.Empty;

    public string Unit { get; set; }
        = string.Empty;

    public decimal Price { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime CreatedAt { get; set; }
}