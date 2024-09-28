using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using Webapi.Libs.User;

namespace Webapi.Libs {
    public class DatabaseContext : DbContext {
        private readonly string connectionString;

        public DatabaseContext(DbContextOptions<DatabaseContext> options, string connectionString) : base(options) {
            this.connectionString = connectionString;
        }

        // public DbSet<User> Users { get; set; }

        public MySqlConnection GetConnection() {
            return new MySqlConnection(connectionString);
        }
    }
}