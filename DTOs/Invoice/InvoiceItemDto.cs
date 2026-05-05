namespace backend_inventory.DTOs.Invoice;

public class InvoiceItemDto
{
    public int Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int QtyReceived { get; set; }
    public string Unit { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Subtotal => QtyReceived * Price;
}