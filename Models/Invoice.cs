using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("invoices")]
public class Invoice
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("invoice_number")]
    [MaxLength(50)]
    public string InvoiceNumber { get; set; } = string.Empty;

    [Column("receive_goods_id")]
    public int ReceiveGoodsId { get; set; }

    [Column("invoice_date")]
    public DateTime InvoiceDate { get; set; }

    [Column("invoice_ref")]
    [MaxLength(100)]
    public string? InvoiceRef { get; set; }

    [Column("notes")]
    [MaxLength(500)]
    public string? Notes { get; set; }

    [Column("status")]
    [MaxLength(10)]
    public string Status { get; set; } = "DRAFT"; // DRAFT, SENT

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigasi
    [ForeignKey("ReceiveGoodsId")]
    public ReceiveGoods? ReceiveGoods { get; set; }
    public ICollection<InvoiceItem> Items { get; set; } = new List<InvoiceItem>();
}