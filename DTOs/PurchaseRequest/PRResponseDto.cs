namespace backend_inventory.DTOs.PurchaseRequest;

public class PRResponseDto
{
    public int Id { get; set; }
    public string PrNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Pic { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public DateTime NeededDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<PRItemDto> Items { get; set; } = new();
    public decimal GrandTotal => Items.Sum(i => i.Subtotal);
}