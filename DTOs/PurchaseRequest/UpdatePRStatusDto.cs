using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.PurchaseRequest;

public class UpdatePRStatusDto
{
    [Required]
    [RegularExpression("^(WAITING_APPROVAL|APPROVED|REJECTED)$",
        ErrorMessage = "Status harus WAITING_APPROVAL, APPROVED, atau REJECTED")]
    public string Status { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Comment { get; set; }
}