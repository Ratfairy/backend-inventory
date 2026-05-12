using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("stock_movements")]
public class StockMovement
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("stock_id")]
    public int StockId { get; set; }

    [Column("type")]
    [MaxLength(10)]
    public string Type { get; set; } = string.Empty; // IN / OUT

    [Column("qty")]
    public int Qty { get; set; }

    [Column("description")]
    [MaxLength(255)]
    public string? Description { get; set; }

    [Column("pic")]
    [MaxLength(100)]
    public string? Pic { get; set; }

    [Column("date")]
    public DateTime Date { get; set; } = DateTime.UtcNow;

    // Navigasi
    [ForeignKey("StockId")]
    public Stock? Stock { get; set; }

    [Column("reference_type")]
    [MaxLength(50)]
    public string? ReferenceType { get; set; }
}