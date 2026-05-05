namespace backend_inventory.DTOs.PurchaseRequest;

public class PRItemDto
{
    public int Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int Qty { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Subtotal => Qty * Price;
    public string? Reason { get; set; }
}