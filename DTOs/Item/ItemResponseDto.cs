namespace backend_inventory.DTOs.Item;

public class ItemResponseDto
{
    public int Id { get; set; }

    public int CategoryId { get; set; }

    public string CategoryName { get; set; }
        = string.Empty;

    public string ItemName { get; set; }
        = string.Empty;

    public string Unit { get; set; }
        = string.Empty;

    public decimal Price { get; set; }

    public string Status { get; set; }
        = string.Empty;

    public string? CreatedBy { get; set; }

    public bool HasStock { get; set; }

    public DateTime CreatedAt { get; set; }
}