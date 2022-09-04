using Common;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolManagement;
using Service;
using Service.IService;
using Service.Model.Product;

namespace ShoesEShop.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly IProductService _productService;

    public ProductsController(DatabaseContext databaseContext, IWebHostEnvironment webHostEnvironment, IProductService productService)
    {
        _databaseContext = databaseContext;
        _webHostEnvironment = webHostEnvironment;
        _productService = productService;
    }


    [HttpGet("{productId:int}")]
    public async Task<ApiResult<GetFullProduct>> GetProduct(int productId)
    {
        var product = await _databaseContext.Products.Where(p => p.Id == productId)
            .Select(p => new GetFullProduct()
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                SubPhoto = p.SubPicture,
                MainPhoto = p.MainPicture,
                CategoryNames = p.Categories.Select(c => c.Name).ToList(),
                Sizes = p.Sizes,
            }).FirstOrDefaultAsync();

        return new ApiResult<GetFullProduct>()
        {
            Data = product,
            IsSuccess = true,
            StatusCode = ApiResultStatusCode.Success,
            Message = new[] {"عملیات با موفقیت انجام شد"}
        };
    }

    [HttpGet]
    public async Task<ApiResult<List<GetProductMini>>> GetAll([FromQuery] int? categoryId, [FromQuery] string? name, [FromQuery] int? minPrice, [FromQuery] int? maxPrice)
    {
        var products = _databaseContext.Products.AsQueryable();
        if (categoryId != null)
            products = products.Where(c => c.Categories.Select(s => s.Id).Contains((int) categoryId)).AsQueryable();
        if (!string.IsNullOrEmpty(name))
            products = products.Where(c => c.Name.Contains(name)).AsQueryable();
        if (minPrice != null)
            products = products.Where(c => c.Price >= minPrice).AsQueryable();
        if (maxPrice != null)
            products = products.Where(c => c.Price <= maxPrice).AsQueryable();

        var data = await products.Select(p => new GetProductMini()
        {
            Price = p.Price,
            Id = p.Id,
            MainPhoto = p.MainPicture,
            Name = p.Name,
            CategoryName = p.Categories.Where(c => c.ParentId == null).Select(s => s.Name).FirstOrDefault()!,
            Sizes = p.Sizes,
        }).ToListAsync();

        return new ApiResult<List<GetProductMini>>()
        {
            Data = data,
            IsSuccess = true,
            StatusCode = ApiResultStatusCode.Success,
            Message = new[] {"عملیات با موفقیت انجام شد"}
        };
    }

    [Authorize]
    [HttpPost]
    public async Task<ApiResult> CreateProduct([FromForm] AddProduct addProduct)
    {

        var product = new Product()
        {
            Price = addProduct.Price,
            Description = addProduct.Description,
            InStock = addProduct.InStock,
            //MainPicture = mainPhotoUrl.Data,
            Name = addProduct.Name,
            //SubPicture = subPhotosUrl.Data,
            Orders = new List<ProductInOrder>(),
            //Categories = categories,
            Sizes = addProduct.Sizes,
        };

        if (addProduct.CategoriesId != null && addProduct.CategoriesId.Count > 0)
        {
            var categories = await _databaseContext.Categories.Where(c => addProduct.CategoriesId.Contains(c.Id))
                .ToListAsync();
            product.Categories = categories;
        }

        if (addProduct.CategoriesId != null && addProduct.CategoriesId.Count > 0)
        {
            var categories = await _databaseContext.Categories.Where(c => addProduct.CategoriesId.Contains(c.Id))
                .ToListAsync();
            product.Categories = categories;
        }

        if (addProduct.MainPhoto != null)
        {
            var mainPhotoUrl = await _productService.SaveFiles(_webHostEnvironment, addProduct.MainPhoto);
            if (mainPhotoUrl.IsSuccess == false)
                return mainPhotoUrl;

            product.MainPicture = mainPhotoUrl.Data;
        }

        if (addProduct.SubPhoto != null && addProduct.SubPhoto.Count > 0)
        {
            var subPhotosUrl = await _productService.SaveFiles(_webHostEnvironment, addProduct.SubPhoto);
            if (subPhotosUrl.IsSuccess == false)
                return subPhotosUrl;
            product.SubPicture = subPhotosUrl.Data;
        }

        await _databaseContext.Products.AddAsync(product);
        await _databaseContext.SaveChangesAsync();

        return new ApiResult()
        { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "محصول اضافه شد" } };

    }

    [HttpPut("{productId}")]
    public async Task<ApiResult> EditProduct([FromRoute] int productId, [FromBody] EditProduct editProduct)
    {
        if (productId != editProduct.Id)
        {
            return new ApiResult()
                { IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest, Message = new[] { "مغایرتی در داده های ارسالی وجود دارد" } };
        }

        var product = await _databaseContext.Products.Where(p => p.Id == productId).Include(c => c.Categories).AsTracking().FirstOrDefaultAsync();

        if (editProduct.InStock != null )
        {
            product.InStock = (int)editProduct.InStock;
        }

        if (!string.IsNullOrEmpty(editProduct.Name))
        {
            product.Name = editProduct.Name;
        }

        if (!string.IsNullOrEmpty(editProduct.Description))
        {
            product.Description = editProduct.Description;
        }

        if (editProduct.Price != null)
        {
            product.Price = (int) editProduct.Price;
        }

        if (editProduct.CategoriesId != null && editProduct.CategoriesId.Count > 0)
        {
            var categories = await _databaseContext.Categories.Where(c => editProduct.CategoriesId.Contains(c.Id))
                .ToListAsync();
            product.Categories = categories;
        }

        if (editProduct.Sizes != null && editProduct.Sizes.Count > 0)
        {
            product.Sizes = editProduct.Sizes;
        }

        await _databaseContext.SaveChangesAsync();
        return new ApiResult()
        { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "محصول ویرایش شد" } };

    }
}