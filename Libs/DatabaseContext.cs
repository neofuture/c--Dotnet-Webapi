using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;

namespace Webapi.Libs {
    public class DatabaseContext(DbContextOptions<DatabaseContext> options, string connectionString) : DbContext(options) {
        private readonly string connectionString = connectionString;
        
        public MySqlConnection GetConnection() {
            return new MySqlConnection(connectionString);
        }
    }
}