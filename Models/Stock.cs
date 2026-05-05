using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("stocks")]
public class Stock
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("item_name")]
    [Required]
    [MaxLength(100)]
    public string ItemName { get; set; } = string.Empty;

    [Column("qty")]
    public int Qty { get; set; } = 0;

    [Column("min_qty")]
    public int MinQty { get; set; } = 0;

    [Column("unit")]
    [Required]
    [MaxLength(50)]
    public string Unit { get; set; } = string.Empty;

    [Column("price", TypeName = "decimal(18,2)")]
    public decimal Price { get; set; } = 0;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigasi
    public ICollection<StockMovement> Movements { get; set; } = new List<StockMovement>();
    public ICollection<Adjustment> Adjustments { get; set; } = new List<Adjustment>();
    public ICollection<StockOpnameItem> OpnameItems { get; set; } = new List<StockOpnameItem>();
}