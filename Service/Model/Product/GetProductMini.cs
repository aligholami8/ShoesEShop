using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model.Product;

public class GetProductMini
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MainPhoto { get; set; }
    public int Price { get; set; }
    public string CategoryName { get; set; }
    public List<string> Sizes { get; set; }
}

public class GetProductMiniForOrder
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string MainPhoto { get; set; }
    public int Price { get; set; }
    public string CategoryName { get; set; }
    public string Sizes { get; set; }
    public int Count { get; set; }
}