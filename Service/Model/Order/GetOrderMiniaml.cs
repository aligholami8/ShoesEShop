using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Service.Model.Product;

namespace Service.Model.Order;

public class GetOrderMiniaml
{
    public int OrderId { get; set; }
    public string Address { get; set; }
    public bool IsFinilized { get; set; }
    public List<GetProductMiniForOrder> Products { get; set; }
    public int Count { get; set; }
    public string Size { get; set; }
    public int Price { get; set; }
}