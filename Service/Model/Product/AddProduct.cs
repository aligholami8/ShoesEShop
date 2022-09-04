using Microsoft.AspNetCore.Http;

namespace Service.Model.Product;

public class AddProduct
{
    public string Name { get; set; }
    public string Description { get; set; }
    public int InStock { get; set; }
    public IFormFile? MainPhoto { get; set; }
    public List<IFormFile>? SubPhoto { get; set; }
    public int Price { get; set; }
    public List<int>? CategoriesId { get; set; }
    public List<string>? Sizes { get; set; }
}