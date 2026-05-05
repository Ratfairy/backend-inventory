using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("purchase_order_items")]
public class PurchaseOrderItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("purchase_order_id")]
    public int PurchaseOrderId { get; set; }

    [Column("item_name")]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Column("qty")]
    public int Qty { get; set; }

    [Column("unit")]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    [Column("reason")]
    [MaxLength(255)]
    public string? Reason { get; set; }

    // Navigasi
    [ForeignKey("PurchaseOrderId")]
    public PurchaseOrder? PurchaseOrder { get; set; }
}