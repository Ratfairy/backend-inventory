using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("adjustment_items")]
public class AdjustmentItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("adjustment_id")]
    public int AdjustmentId { get; set; }

    [Column("stock_id")]
    public int StockId { get; set; }

    [Column("system_qty")]
    public int SystemQty { get; set; }

    [Column("actual_qty")]
    public int ActualQty { get; set; }

    [Column("adjustment_qty")]
    public int AdjustmentQty { get; set; }

    // Navigation
    [ForeignKey("AdjustmentId")]
    public Adjustment? Adjustment { get; set; }

    [ForeignKey("StockId")]
    public Stock? Stock { get; set; }
}