using Microsoft.AspNetCore.Mvc; // Add this line
using Webapi.Libs.User;

namespace Webapi.Libs {
    public static class App {
        public static void ConfigureEndpoints(this WebApplication app) {
            app.MapPost("/user/login",
                    async ([FromBody] LoginRequest loginRequest, [FromServices] UserLogin userLogin)
                        => await userLogin.LoginAsync(loginRequest)
                )
                .WithName("User/Login")
                .WithOpenApi();
        }
    }
}