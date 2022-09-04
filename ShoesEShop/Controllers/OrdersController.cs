using Common;
using Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolManagement;
using Service;
using Service.Model.Order;
using Service.Model.Product;

namespace ShoesEShop.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly DatabaseContext _databaseContext;

    public OrdersController(DatabaseContext databaseContext)
    {
        _databaseContext = databaseContext;
    }


    [HttpGet("active-order")]
    public async Task<ApiResult<GetOrderMiniaml?>> GetActiveOrder()
    {
        var data = await _databaseContext.Orders
            .Where(c => c.IsFinished == false && c.UserId == User.Identity.GetUserId())
            .Select(o => new GetOrderMiniaml()
            {
                Products = o.Products
                    .Select(p => new GetProductMiniForOrder()
                    {
                        Price = p.Product.Price,
                        Id = p.Id,
                        MainPhoto = p.Product.MainPicture,
                        Name = p.Product.Name,
                        CategoryName = p.Product.Categories
                            .Where(c => c.ParentId == null)
                            .Select(s => s.Name).FirstOrDefault()!,
                        Sizes = p.Size,
                        Count = p.Count,
                    }).ToList(),
                Address = o.Address,
                IsFinilized = o.IsFinished,
                OrderId = o.Id,
                Price = o.Price,
            })
            .FirstOrDefaultAsync();
        return new ApiResult<GetOrderMiniaml?>()
        {
            IsSuccess = true,
            Message = new[] { "عملیات موفقیت آمیز بود" },
            StatusCode = ApiResultStatusCode.Success,
            Data = data,
        };
    }

    [HttpGet("all-orders")]
    public async Task<ApiResult<List<GetOrderMiniaml>>> GetAllOrders()
    {
        var data = await _databaseContext.Orders
            .Where(c => c.UserId == User.Identity.GetUserId())
            .Select(o => new GetOrderMiniaml()
            {
                Products = o.Products
                    .Select(p => new GetProductMiniForOrder()
                    {
                        Price = p.Product.Price,
                        Id = p.Id,
                        MainPhoto = p.Product.MainPicture,
                        Name = p.Product.Name,
                        CategoryName = p.Product.Categories
                            .Where(c => c.ParentId == null)
                            .Select(s => s.Name).FirstOrDefault()!,
                        Sizes = p.Size,
                        Count = p.Count,
                    }).ToList(),
                Address = o.Address,
                IsFinilized = o.IsFinished,
                OrderId = o.Id,
                Price = o.Price,
            })
            .ToListAsync();
        return new ApiResult<List<GetOrderMiniaml>>()
        {
            IsSuccess = true,
            Message = new[] { "عملیات موفقیت آمیز بود" },
            StatusCode = ApiResultStatusCode.Success,
            Data = data,
        };
    }

    [HttpPut("add-product-to-order")]
    public async Task<ApiResult> AddProductToOrder(int productId, string size)
    {
        var product = await _databaseContext.Products.Where(p => p.Id == productId && p.InStock > 0)
            .FirstOrDefaultAsync();

        if (product is null)
        {
            throw new NotFoundException();
        }

        var order = await _databaseContext.Orders
            .Include(o => o.Products)
            .Where(c => c.IsFinished == false && c.UserId == User.Identity.GetUserId()).FirstOrDefaultAsync();
        var flag = order != null;

        if (order is null)
        {
            order = new Order()
            {
                Price = 0,
                UserId = User.Identity.GetUserId(),
                IsFinished = false,
                Products = new List<ProductInOrder>(),
                Address = "",
            };
        }

        if (order.Products.Any(c => c.ProductId == productId && c.Size == size))
        {
            order.Products.FirstOrDefault(c => c.ProductId == productId && c.Size == size).Count += 1;
        }
        else
        {
            order.Products.Add(new ProductInOrder()
            {
                Order = order,
                Product = product,
                OrderId = order.Id,
                ProductId = product.Id,
                Size = size,
                Count = 1,
            });
        }

        if (flag is false)
        {
            await _databaseContext.AddAsync(order);
        }

        product.InStock -= 1;
        order.Price += product.Price;

        await _databaseContext.SaveChangesAsync();

        return new ApiResult()
        { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "محصول اضافه شد" } };
    }

    [HttpPut("finalize-order/{orderId}")]
    public async Task<ApiResult> FinalizeOrder([FromRoute] int orderId, [FromBody] FinilizeOrderDto finilizeOrderDto)
    {
        var order = await _databaseContext.Orders
            .Where(c => c.IsFinished == false && c.UserId == User.Identity.GetUserId() && c.Id == orderId)
            .FirstOrDefaultAsync();
        order.IsFinished = true;
        order.Address = finilizeOrderDto.Address;
        await _databaseContext.SaveChangesAsync();
        return new ApiResult()
        { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "سفارش شما با موفقیت ثبت شد" } };
    }


    [HttpDelete("remove-product-from-order")]
    public async Task<ApiResult> RemoveProductToOrder(int productId)
    {
        var product = await _databaseContext.ProductInOrders.Where(p => p.Id == productId)
            .FirstOrDefaultAsync();
        var orgProduct = await _databaseContext.Products.Where(c => c.Id == product.ProductId).FirstOrDefaultAsync();

        if (product is null)
        {
            throw new NotFoundException();
        }

        var order = await _databaseContext.Orders
            .Include(o => o.Products)
            .Where(c => c.IsFinished == false && c.UserId == User.Identity.GetUserId()).FirstOrDefaultAsync();

        if (order is null)
        {
            return new ApiResult()
            { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "Ok!" } };
        }

        orgProduct.InStock += order.Products.First(c => c.Id == product.Id).Count;
        order.Price -= orgProduct.Price * order.Products.First(c => c.Id == product.Id).Count;
        order.Products.Remove(product);
        await _databaseContext.SaveChangesAsync();
        return new ApiResult()
        { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "محصول حذف شد" } };
    }
}