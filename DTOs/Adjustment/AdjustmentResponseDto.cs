namespace backend_inventory.DTOs.Adjustment;

public class AdjustmentResponseDto
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int AdjustmentQty { get; set; }
    public string? Reason { get; set; }
    public string? Pic { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}