using MySql.Data.MySqlClient;

namespace Webapi.Libs.User {
    public class UserLogin(DatabaseContext _dbContext) {
        public async Task<IResult> LoginAsync(LoginRequest loginRequest) {
            var user = GetUser(loginRequest.Email, loginRequest.Password);
            return user != null ? Results.Ok(user) : Results.Unauthorized();
        }

        private User? GetUser(string email, string password) {
            using var connection = _dbContext.GetConnection();
            connection.Open();

            var query = """
                        SELECT id, name, email, password
                        FROM Users
                        WHERE email = @Email
                        AND password = @Password
                        """;

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Email", email);
            command.Parameters.AddWithValue("@Password", password);

            using var reader = command.ExecuteReader();
            if (reader.Read()) {
                return new User {
                    Id = reader.GetInt32("Id"),
                    Email = reader.GetString("Email"),
                    Password = reader.GetString("Password")
                };
            }

            return null;
        }
    }
}