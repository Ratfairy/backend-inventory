namespace backend_inventory.DTOs.StockOpname;

public class StockOpnameResponseDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
    public string Pic { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public List<StockOpnameItemResponseDto> Items { get; set; } = new();
}

public class StockOpnameItemResponseDto
{
    public int Id { get; set; }
    public int StockId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int SystemQty { get; set; }
    public int ActualQty { get; set; }
    public int Adjustment { get; set; }
}