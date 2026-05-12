using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("adjustments")]
public class Adjustment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("reason")]
    [MaxLength(255)]
    public string? Reason { get; set; }

    [Column("pic")]
    [MaxLength(100)]
    public string? Pic { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "WAITING"; // WAITING, APPROVED, REJECTED

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("date")]
    public DateTime Date { get; set; }

    public ICollection<AdjustmentItem> Items { get; set; }
        = new List<AdjustmentItem>();

}