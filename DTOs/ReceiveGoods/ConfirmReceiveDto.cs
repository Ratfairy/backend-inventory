using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.ReceiveGoods;

public class ConfirmReceiveDto
{
    [Required]
    [MinLength(1)]
    public List<ConfirmReceiveItemDto> Items { get; set; } = new();
}

public class ConfirmReceiveItemDto
{
    [Required]
    public int ReceiveGoodsItemId { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int QtyReceived { get; set; }

    [MaxLength(255)]
    public string? Note { get; set; }
}