using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("receive_goods")]
public class ReceiveGoods
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("purchase_order_id")]
    public int PurchaseOrderId { get; set; }

    [Column("status")]
    [MaxLength(10)]
    public string Status { get; set; } = "PENDING"; // PENDING, PARTIAL, RECEIVED

    [Column("received_date")]
    public DateTime? ReceivedDate { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigasi
    [ForeignKey("PurchaseOrderId")]
    public PurchaseOrder? PurchaseOrder { get; set; }
    public ICollection<ReceiveGoodsItem> Items { get; set; } = new List<ReceiveGoodsItem>();
    public Invoice? Invoice { get; set; }
}