using Microsoft.EntityFrameworkCore;

namespace PaymentGateway.Models{
    public class LogContext : DbContext{
        public LogContext(DbContextOptions<LogContext> options) : base(options) {}

        public DbSet<Log> Log  {get; set;}
    }
}