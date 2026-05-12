using Microsoft.AspNetCore.Mvc;

using backend_inventory.DTOs.Category;
using backend_inventory.Interfaces;

namespace backend_inventory.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoryController : ControllerBase
{
    private readonly ICategoryService _categoryService;

    public CategoryController(
        ICategoryService categoryService
    )
    {
        _categoryService = categoryService;
    }

    // =====================
    // GET ALL
    // =====================

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result =
            await _categoryService.GetAllCategoriesAsync();

        return Ok(result);
    }

    // =====================
    // GET BY ID
    // =====================

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var result =
            await _categoryService.GetCategoryByIdAsync(id);

        if (result == null)
        {
            return NotFound(
                new { message = "Category tidak ditemukan" }
            );
        }

        return Ok(result);
    }

    // =====================
    // CREATE
    // =====================

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateCategoryDto dto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result =
                await _categoryService.CreateCategoryAsync(dto);

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new { message = ex.Message }
            );
        }
    }

    // =====================
    // UPDATE
    // =====================

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(
        int id,
        [FromBody] UpdateCategoryDto dto
    )
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        try
        {
            var result =
                await _categoryService.UpdateCategoryAsync(
                    id,
                    dto
                );

            if (result == null)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Category tidak ditemukan"
                    }
                );
            }

            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(
                new { message = ex.Message }
            );
        }
    }

    // =====================
    // DELETE
    // =====================

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var result =
                await _categoryService.DeleteCategoryAsync(id);

            if (!result)
            {
                return NotFound(
                    new
                    {
                        message =
                            "Category tidak ditemukan"
                    }
                );
            }

            return Ok(
                new
                {
                    message =
                        "Category berhasil dihapus"
                }
            );
        }
        catch (Exception ex)
        {
            return BadRequest(
                new { message = ex.Message }
            );
        }
    }
}