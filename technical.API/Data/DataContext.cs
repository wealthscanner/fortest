using Microsoft.EntityFrameworkCore;
using technical.API.Models;

namespace technical.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        { }

        public DbSet<Value> Values { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Photo> Photos { get; set; }


        public DbSet<Log> Logging { get; set; }
    }
}
