namespace Webapi.Libs.User {
    public static class ServiceCollectionExtensions {
        public static void AddUserLogin(this IServiceCollection services, string jwtSecret) {
            services.AddScoped<UserLogin>(provider => new UserLogin(
                provider.GetRequiredService<DatabaseContext>(),
                jwtSecret
            ));
        }
    }
}