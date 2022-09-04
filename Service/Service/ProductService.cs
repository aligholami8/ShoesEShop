using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using PoolManagement;
using Service.IService;

namespace Service.Service;

public class ProductService : IProductService
{
    public async Task<ApiResult<string>> SaveFiles(IWebHostEnvironment env, IFormFile file)
    {
        try
        {
            var filename = Guid.NewGuid() + Path.GetExtension(file.FileName)?.ToLower();
            var path = Path.Combine(env.WebRootPath, "images", filename);

            await using (var stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var url = $"{"/images/"}{filename}";

            return new ApiResult<string>()
                { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "" }, Data = url };
        }
        catch (Exception e)
        {
            var msg =
                $"{DateTime.Now} :MESSAGE :  File Upload Error : \n MESSAGE : {e.Message} \n , INNER : {e.InnerException}";
            return new ApiResult<string>()
                { IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest, Message = new[] { msg }, Data = ""};
        }
    }

    public async Task<ApiResult<List<string>>> SaveFiles(IWebHostEnvironment env, List<IFormFile> files)
    {
        var urls = new List<string>();
        foreach (var file in files)
        {
            try
            {
                var filename = Guid.NewGuid() + Path.GetExtension(file.FileName)?.ToLower();
                var path = Path.Combine(env.WebRootPath, "images", filename);

                await using (var stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
                var url = $"{"/images/"}{filename}";
                urls.Add(url);
                
            }
            catch (Exception e)
            {
                var msg =
                    $"{DateTime.Now} :MESSAGE :  File Upload Error : \n MESSAGE : {e.Message} \n , INNER : {e.InnerException}";
                return new ApiResult<List<string>>()
                    { IsSuccess = false, StatusCode = ApiResultStatusCode.BadRequest, Message = new[] { msg }, Data = new List<string>() };
            }
        }

        return new ApiResult<List<string>>()
            { IsSuccess = true, StatusCode = ApiResultStatusCode.Success, Message = new[] { "" }, Data = urls };
    }
}