
using AuthService.BO.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace AuthService.DAO
{
    public class AuthServiceDbContext : DbContext
    {
        string _connectionString;
        public AuthServiceDbContext()
        {
            _connectionString = CustomerConnectionString();
        }

        public AuthServiceDbContext(DbContextOptions<AuthServiceDbContext> options) : base(options)
        {
        }

        private string CustomerConnectionString()
        {
            var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
            var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "3306";
            var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "prn232";
            var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "root";
            var dbPass = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "Nghia_2003";
            var connectionString = $"Server={dbHost};Port={dbPort};Database={dbName};User Id={dbUser};Password={dbPass};SslMode=Required;";
            return connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) => optionsBuilder.UseMySql(CustomerConnectionString(),
            new MySqlServerVersion(new Version(8, 0, 31)),
            mySqlOptions =>
                mySqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 5,
                    maxRetryDelay: TimeSpan.FromSeconds(10),
                    errorNumbersToAdd: null
                ));

        public virtual DbSet<Account> Accounts { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}