using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_inventory.Models;

[Table("items")]
public class Item
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("category_id")]
    public int CategoryId { get; set; }

    [Column("item_name")]
    public string ItemName { get; set; } = string.Empty;

    [Column("unit")]
    public string Unit { get; set; } = string.Empty;

    [Column("price")]
    public decimal Price { get; set; }

    [Column("status")]
    public string Status { get; set; } = "WAITING";

    [Column("created_by")]
    public string? CreatedBy { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public Category Category { get; set; } = null!;

    public Stock? Stock { get; set; }
}