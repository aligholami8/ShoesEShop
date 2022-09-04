
namespace Data;

public class Order
{
    public int Id { get; set; }
    public List<ProductInOrder> Products { get; set; }
    public bool IsFinished { get; set; }

    public User User { get; set; }
    public string UserId { get; set; }

    public string Address { get; set; }
    public int Price { get; set; }
}

public class ProductInOrder
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }

    public int Count { get; set; }
    public string Size { get; set; }

    public Product Product { get; set; }
    public Order Order { get; set; }

}