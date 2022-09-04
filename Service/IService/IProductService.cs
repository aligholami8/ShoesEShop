using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Service.IService;

public interface IProductService
{
    Task<ApiResult<string>> SaveFiles(IWebHostEnvironment env, IFormFile file);
    Task<ApiResult<List<string>>> SaveFiles(IWebHostEnvironment env, List<IFormFile> files);
}