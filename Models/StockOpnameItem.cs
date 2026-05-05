using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("stock_opname_items")]
public class StockOpnameItem
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("stock_opname_id")]
    public int StockOpnameId { get; set; }

    [Column("stock_id")]
    public int StockId { get; set; }

    [Column("system_qty")]
    public int SystemQty { get; set; }

    [Column("actual_qty")]
    public int ActualQty { get; set; }

    [Column("adjustment")]
    public int Adjustment { get; set; }

    // Navigasi
    [ForeignKey("StockOpnameId")]
    public StockOpname? StockOpname { get; set; }

    [ForeignKey("StockId")]
    public Stock? Stock { get; set; }
}