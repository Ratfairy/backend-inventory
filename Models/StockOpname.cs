using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("stock_opnames")]
public class StockOpname
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("date")]
    public DateTime Date { get; set; }

    [Column("pic")]
    [MaxLength(100)]
    public string Pic { get; set; } = string.Empty;

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "WAITING"; // WAITING, APPROVED, REJECTED

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigasi
    public ICollection<StockOpnameItem> Items { get; set; } = new List<StockOpnameItem>();
}