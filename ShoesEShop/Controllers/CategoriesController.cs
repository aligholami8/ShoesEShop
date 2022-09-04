using Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolManagement;
using Service;
using Service.Model.Category;

namespace ShoesEShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CategoriesController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public CategoriesController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }

    [HttpGet]
    public async Task<ApiResult<List<GetCategory>>> Get()
    {
        var categories = await _databaseContext.Categories
            .Include(c => c.SubCategories)
            .Select(c => new GetCategory()
            {
                Id = c.Id,
                Name = c.Name,
                SubCategories = c.SubCategories.Select(x => new GetCategory() {Id = x.Id, Name = x.Name}).ToList(),
            }).ToListAsync();

        return new ApiResult<List<GetCategory>>()
        {
            IsSuccess = true,
            Data = categories,
            Message = new[] {"موفقیت آمیز بود"},
            StatusCode = ApiResultStatusCode.Success,
        };
    }

    [HttpPost]
    public async Task<ApiResult> Post(AddCategory addCategory)
    {
        var category = new Category()
        {
            Name = addCategory.Name,
            ParentId = addCategory.ParentId,
        };

        await _databaseContext.AddAsync(category);
        await _databaseContext.SaveChangesAsync();

        return new ApiResult()
            { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "کتگوری اضافه شد" } };
    }

    [HttpPut]
    public async Task<ApiResult> Edit([FromBody] EditCategory editCategory)
    {
        var category = await _databaseContext.Categories.Where(c => c.Id == editCategory.Id).FirstOrDefaultAsync();
        category.Name = editCategory.Name;
        category.ParentId = editCategory.ParentId;
        
        await _databaseContext.SaveChangesAsync();
        return new ApiResult()
            { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "کتگوری ویرایش شد" } };
    }
}