using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace MVC.Data
{
    public class MVCContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public MVCContext() : base("name=MVCContext")
        {
        }

        public System.Data.Entity.DbSet<MVC.Models.Quarto> Quartos { get; set; }
        public System.Data.Entity.DbSet<MVC.Models.Cliente> Clientes { get; set; }
        public System.Data.Entity.DbSet<MVC.Models.Estadia> Estadias { get; set; }
        public System.Data.Entity.DbSet<MVC.Models.User> Users { get; set; }
    }
}
