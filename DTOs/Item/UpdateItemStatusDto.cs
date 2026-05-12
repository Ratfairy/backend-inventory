using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Item;

public class UpdateItemStatusDto
{
    [Required]
    [RegularExpression(
        "^(APPROVED|REJECTED)$",
        ErrorMessage =
            "Status harus APPROVED atau REJECTED"
    )]
    public string Status { get; set; } = string.Empty;
}