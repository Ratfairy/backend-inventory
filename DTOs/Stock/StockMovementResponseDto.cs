namespace backend_inventory.DTOs.Stock;

public class StockMovementResponseDto
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty;
    public int Qty { get; set; }
    public string? Description { get; set; }
    public string? Pic { get; set; }
    public DateTime Date { get; set; }
}