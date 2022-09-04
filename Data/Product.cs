namespace Data;

public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public List<string> Sizes { get; set; }

    public int Price { get; set; }

    public List<Category> Categories { get; set; }

    public int InStock { get; set; }

    public string MainPicture { get; set; }
    public List<string> SubPicture { get; set; }

    public List<ProductInOrder> Orders { get; set; }
}