using Microsoft.EntityFrameworkCore;
using sistema_de_inventario_y_ventas.Context.Entitys;

namespace sistema_de_inventario_y_ventas.Context
{
        public class AppDbContext : DbContext
        {
                public AppDbContext( DbContextOptions options ) : base(options) {
                }

                protected AppDbContext() {
                }

                public DbSet<Laptop> Laptops { get; set;      }
        }
}
