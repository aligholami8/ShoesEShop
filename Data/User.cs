using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data;
using Microsoft.AspNetCore.Identity;

namespace Data
{
    public class User : IdentityUser
    {
        public string FullName { get; set; }

        public List<Order> Orders { get; set; }

    }
}
