using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Models{
    public class ShopperContext : DbContext{
        public ShopperContext(DbContextOptions<ShopperContext> options) : base(options) {}

        public DbSet<Shopper> Shopper {get; set;}
    }
}