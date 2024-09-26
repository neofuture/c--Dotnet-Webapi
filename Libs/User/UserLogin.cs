using MySql.Data.MySqlClient;

namespace Webapi.Libs.User {
    public class UserLogin(DatabaseContext dbContext) {
        
        public async Task<IResult> LoginAsync(LoginRequest loginRequest) {
            var user = await GetUserAsync(loginRequest.Email, loginRequest.Password);
            return user != null ? Results.Ok(user) : Results.Unauthorized();
        }

        private async Task<User?> GetUserAsync(string email, string password) {
            await using var connection = dbContext.GetConnection();
            await connection.OpenAsync();

            var query = """
                        SELECT id, name, email, password
                        FROM Users
                        WHERE email = @Email
                        AND password = @Password
                        """;

            await using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            await using var reader = await command.ExecuteReaderAsync();
            if (await reader.ReadAsync()) {
                return new User {
                    Id = reader.GetInt32(0), // Column index for "Id"
                    Email = reader.GetString(2), // Column index for "Email"
                    Password = reader.GetString(3) // Column index for "Password"
                };
            }

            return null;
        }
    }
}