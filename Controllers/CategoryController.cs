using ERP.Shared.Categories;
using ERP.Shared.Commons;
using ERP.Ticketing.HttpApi.Controllers.Common;
using ERP.Ticketing.HttpApi.Features.Categories;
using Microsoft.AspNetCore.Mvc;

namespace ERP.Ticketing.HttpApi.Controllers;

[Route("api/ticketing/categories")]
public class CategoryController: AuthApiControllerBase
{
    private readonly CategoryService _categoryService;

    public CategoryController(CategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    [HttpPost("list", Name = "categories_list")]
    public async Task<PagedResult<CategoryDto>> List(CategoryListFilter filters)
    {
        return await _categoryService.ListOfCategory(filters);
    }
    
    [HttpGet("{categoryId:guid}", Name = "get_category_by_id")]
    public async Task<CategoryDto> GetById(Guid categoryId)
    {
        return await _categoryService.GetById(categoryId);
    }
    
    [HttpPost(Name = "create_category")]
    public async Task<IActionResult> Create(CategoryCreateDto request)
    {
        await _categoryService.Create(request);
        return Ok();
    }
    
    [HttpPatch("{categoryId:guid}", Name = "update_category")]
    public async Task<IActionResult> Update(Guid categoryId, CategoryCreateDto request)
    {
        await _categoryService.Update(categoryId, request);
        return Ok();
    }
    
    [HttpDelete("{categoryId:guid}", Name = "delete_category")]
    public async Task<IActionResult> Delete(Guid categoryId)
    {
        await _categoryService.Delete(categoryId);
        return Ok();
    }
}