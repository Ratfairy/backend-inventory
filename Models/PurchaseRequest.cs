using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("purchase_requests")]
public class PurchaseRequest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("pr_number")]
    [MaxLength(50)]
    public string PrNumber { get; set; } = string.Empty;

    [Column("department")]
    [MaxLength(100)]
    public string Department { get; set; } = string.Empty;

    [Column("pic")]
    [MaxLength(100)]
    public string Pic { get; set; } = string.Empty;

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Column("needed_date")]
    public DateTime NeededDate { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "DRAFT"; // DRAFT, WAITING_APPROVAL, APPROVED, REJECTED

    [Column("comment")]
    [MaxLength(500)]
    public string? Comment { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigasi
    public ICollection<PurchaseRequestItem> Items { get; set; } = new List<PurchaseRequestItem>();
    public PurchaseOrder? PurchaseOrder { get; set; }
}