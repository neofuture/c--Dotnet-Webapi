using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Webapi.Libs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddDbContext<DatabaseContext>(options =>
    options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
        new MySqlServerVersion(new Version(8, 0, 21))));

builder.Services.AddSingleton(builder.Configuration.GetConnectionString("DefaultConnection"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" }); });

var jwtSecret = builder.Configuration.GetSection("JwtSettings:Secret").Value;

if (string.IsNullOrEmpty(jwtSecret)) {
    throw new ArgumentNullException(nameof(jwtSecret), "JWT Secret cannot be null or empty.");
}

builder.Services.AddUserLogin(jwtSecret);

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1"));
}
else {
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