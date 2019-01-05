using Microsoft.EntityFrameworkCore;
using TrustFelix.API.Models;

namespace TrustFelix.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        { }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Log> Logging { get; set; }
    }
}
