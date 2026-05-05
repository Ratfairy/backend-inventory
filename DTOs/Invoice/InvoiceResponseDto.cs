namespace backend_inventory.DTOs.Invoice;

public class InvoiceResponseDto
{
    public int Id { get; set; }
    public string InvoiceNumber { get; set; } = string.Empty;
    public int ReceiveGoodsId { get; set; }
    public string PoNumber { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Pic { get; set; } = string.Empty;
    public DateTime InvoiceDate { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string? InvoiceRef { get; set; }
    public string? Notes { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<InvoiceItemDto> Items { get; set; } = new();
    public decimal GrandTotal => Items.Sum(i => i.Subtotal);
}