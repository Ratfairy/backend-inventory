using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Adjustment;

public class UpdateAdjustmentStatusDto
{
    [Required]
    [RegularExpression("^(APPROVED|REJECTED)$",
        ErrorMessage = "Status harus APPROVED atau REJECTED")]
    public string Status { get; set; } = string.Empty;
} 