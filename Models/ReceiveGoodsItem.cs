using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("receive_goods_items")]
public class ReceiveGoodsItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("receive_goods_id")]
    public int ReceiveGoodsId { get; set; }

    [Column("item_name")]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Column("qty_ordered")]
    public int QtyOrdered { get; set; }

    [Column("qty_received")]
    public int QtyReceived { get; set; } = 0;

    [Column("unit")]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Column("note")]
    [MaxLength(255)]
    public string? Note { get; set; }

    // Navigasi
    [ForeignKey("ReceiveGoodsId")]
    public ReceiveGoods? ReceiveGoods { get; set; }
}