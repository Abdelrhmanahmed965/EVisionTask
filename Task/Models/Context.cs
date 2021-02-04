using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Task.Models
{
    public class Context : DbContext
    {
        public Context()
          : base("ProductConnection")
        {
        }

        public DbSet<Product> products { get; set; }
    }
}