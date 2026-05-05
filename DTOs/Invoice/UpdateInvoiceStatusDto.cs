using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Invoice;

public class UpdateInvoiceStatusDto
{
    [Required]
    [RegularExpression("^(SENT)$",
        ErrorMessage = "Status hanya bisa SENT")]
    public string Status { get; set; } = string.Empty;
}