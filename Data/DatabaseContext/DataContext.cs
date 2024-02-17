using KobaParts.Models.Api.Client;
using KobaParts.Models.Api.Store;
using Microsoft.EntityFrameworkCore;

namespace KobaParts.Data.DatabaseContext
{
    public class DataContext : DbContext
    {
        private readonly string _connectionString;
        private readonly MySqlServerVersion _serverVersion = new(new Version(8, 0, 33));
        public DataContext(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("localDb") ?? string.Empty;
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Order> Orders => Set<Order>();
        public DbSet<FavoritesProduct> Favorties => Set<FavoritesProduct>();
        public DbSet<Purchase> Backets => Set<Purchase>();
        public DbSet<UserType> Roles => Set<UserType>();
        public DbSet<Product> Products => Set<Product>();

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseMySql(_connectionString, _serverVersion)
                .LogTo(Console.WriteLine, LogLevel.Information);
        }
    }
}
