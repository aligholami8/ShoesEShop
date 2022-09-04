using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Model.Category;

public class GetCategory
{
    public int Id { get; set; }
    public string Name { get; set; }

    public List<GetCategory> SubCategories { get; set; }
}