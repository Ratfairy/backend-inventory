using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("purchase_orders")]
public class PurchaseOrder
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("po_number")]
    [MaxLength(50)]
    public string PoNumber { get; set; } = string.Empty;

    [Column("purchase_request_id")]
    public int PurchaseRequestId { get; set; }

    [Column("supplier")]
    [MaxLength(100)]
    public string Supplier { get; set; } = string.Empty;

    [Column("status")]
    [MaxLength(10)]
    public string Status { get; set; } = "DRAFT"; // DRAFT, SENT

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigasi
    [ForeignKey("PurchaseRequestId")]
    public PurchaseRequest? PurchaseRequest { get; set; }
    public ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    public ReceiveGoods? ReceiveGoods { get; set; }
}