using System.ComponentModel.DataAnnotations;

namespace backend_inventory.DTOs.Category;

public class UpdateCategoryDto
{
    [Required]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }
}