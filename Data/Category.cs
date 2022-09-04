using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public int? ParentId { get; set; }
    public Category Parent { get; set; }

    public List<Category> SubCategories { get; set; }

    public List<Product> Products { get; set; }
}