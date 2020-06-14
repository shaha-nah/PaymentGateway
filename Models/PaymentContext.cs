using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Models{
    public class PaymentContext : DbContext{
        public PaymentContext(DbContextOptions<PaymentContext> options) : base(options) {}

        public DbSet<Payment> Payment {get;set;}
    }
}