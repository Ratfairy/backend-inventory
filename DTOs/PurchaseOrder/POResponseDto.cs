namespace backend_inventory.DTOs.PurchaseOrder;

public class POResponseDto
{
    public int Id { get; set; }
    public string PoNumber { get; set; } = string.Empty;
    public int PurchaseRequestId { get; set; }
    public string PrNumber { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Pic { get; set; } = string.Empty;
    public DateTime NeededDate { get; set; }
    public DateTime Date { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<POItemDto> Items { get; set; } = new();
    public decimal GrandTotal => Items.Sum(i => i.Subtotal);
}