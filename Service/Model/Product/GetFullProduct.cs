namespace Service.Model.Product;

public class GetFullProduct
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Price { get; set; }
    public string MainPhoto { get; set; }
    public List<string> SubPhoto { get; set; }

    public List<string> CategoryNames { get; set; }
    public List<string> Sizes { get; set; }
}