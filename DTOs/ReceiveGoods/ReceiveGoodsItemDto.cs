namespace backend_inventory.DTOs.ReceiveGoods;

public class ReceiveGoodsItemDto
{
    public int Id { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public int QtyOrdered { get; set; }
    public int QtyReceived { get; set; }
    public int QtyRemaining => QtyOrdered - QtyReceived;
    public string Unit { get; set; } = string.Empty;
    public string? Note { get; set; }
    public string ItemStatus => QtyReceived >= QtyOrdered
        ? "Lengkap"
        : QtyReceived > 0
        ? "Sebagian"
        : "Belum Diterima";
}