using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("invoice_items")]
public class InvoiceItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("invoice_id")]
    public int InvoiceId { get; set; }

    [Column("item_name")]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Column("qty_received")]
    public int QtyReceived { get; set; }

    [Column("unit")]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    // Navigasi
    [ForeignKey("InvoiceId")]
    public Invoice? Invoice { get; set; }
}