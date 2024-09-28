using MySql.Data.MySqlClient;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace Webapi.Libs.User {
    public class UserLogin(DatabaseContext dbContext, string jwtSecret) {

        public async Task<IResult> LoginAsync(LoginRequest loginRequest) {
            var user = await GetUserAsync(loginRequest.Email, loginRequest.Password);
            if (user != null) {
                var token = GenerateJwtToken(user);
                return Results.Ok(new { user, token });
            }
            var errorResponse = new { error = "Not Authorized" };
            return Results.Json(errorResponse, statusCode: StatusCodes.Status401Unauthorized);
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

        private string GenerateJwtToken(User user) {
            var key = Encoding.ASCII.GetBytes(jwtSecret);

            var payload = new {
                id = user.Id.ToString(),
                email = user.Email,
                permissions = new {
                    first = "yes",
                    second = "no"
                },
                nbf = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds(),
                exp = new DateTimeOffset(DateTime.UtcNow.AddHours(1)).ToUnixTimeSeconds(),
                iat = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeSeconds()
            };

            var securityToken = new JwtSecurityToken(
                claims: null,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            );

            var token = new JwtSecurityTokenHandler().WriteToken(securityToken);
            var tokenParts = token.Split('.');
            var payloadJson = JsonSerializer.Serialize(payload);
            var payloadBase64 = Base64UrlEncoder.Encode(payloadJson);

            return $"{tokenParts[0]}.{payloadBase64}.{tokenParts[2]}";
        }
    }
}