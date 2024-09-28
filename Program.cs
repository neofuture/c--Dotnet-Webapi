using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Webapi.Libs.User;
using Webapi.Libs;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add DbContext configuration
builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

// Add the connection string as a singleton service
builder.Services.AddSingleton(builder.Configuration.GetConnectionString("DefaultConnection"));

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
});

// Add configuration for jwtSecret
var jwtSecret = builder.Configuration.GetSection("JwtSettings:Secret").Value;

// Register UserLogin with the jwtSecret
builder.Services.AddScoped<UserLogin>(provider => new UserLogin(
    provider.GetRequiredService<DatabaseContext>(),
    jwtSecret
));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();
    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllers();

app.ConfigureEndpoints();

app.Run();
