namespace Service.Model.Product;

public class EditProduct
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? InStock { get; set; }
    public int? Price { get; set; }
    public List<int>? CategoriesId { get; set; }
    public List<string>? Sizes { get; set; }
}