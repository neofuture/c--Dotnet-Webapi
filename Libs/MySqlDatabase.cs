using MySql.Data.MySqlClient;

namespace Webapi.Libs {
    public class DatabaseContext {
        private readonly string? _connectionString;

        public DatabaseContext() {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }

        public MySqlConnection GetConnection() {
            if (_connectionString == null) {
                throw new InvalidOperationException("Connection string is not initialized.");
            }

            return new MySqlConnection(_connectionString);
        }
    }
}