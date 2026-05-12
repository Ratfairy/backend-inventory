using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("stocks")]
public class Stock
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("qty")]
    public int Qty { get; set; }

    [Column("min_qty")]
    public int MinQty { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.Now;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Item Item { get; set; } = null!;

    public ICollection<StockMovement> StockMovements { get; set; }
        = new List<StockMovement>();

    public ICollection<AdjustmentItem> AdjustmentItems { get; set; }
        = new List<AdjustmentItem>();
}