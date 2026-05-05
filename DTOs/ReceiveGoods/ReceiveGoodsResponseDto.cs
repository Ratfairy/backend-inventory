namespace backend_inventory.DTOs.ReceiveGoods;

public class ReceiveGoodsResponseDto
{
    public int Id { get; set; }
    public int PurchaseOrderId { get; set; }
    public string PoNumber { get; set; } = string.Empty;
    public string Supplier { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
    public string Pic { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime? ReceivedDate { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<ReceiveGoodsItemDto> Items { get; set; } = new();
}