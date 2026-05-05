using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Invoice;

public class CreateInvoiceDto
{
    [Required]
    public int ReceiveGoodsId { get; set; }

    [Required]
    public DateTime InvoiceDate { get; set; }

    [MaxLength(100)]
    public string? InvoiceRef { get; set; }

    [MaxLength(500)]
    public string? Notes { get; set; }
}