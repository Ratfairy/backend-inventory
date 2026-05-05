using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.PurchaseOrder;

public class UpdatePOStatusDto
{
    [Required]
    [RegularExpression("^(SENT)$",
        ErrorMessage = "Status hanya bisa SENT")]
    public string Status { get; set; } = string.Empty;
}