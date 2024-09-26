using Webapi.Libs;

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel;
using Webapi.Libs.User;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});
builder.Services.AddSingleton<DatabaseContext>();
builder.Services.AddScoped<UserLogin>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Configure endpoints
app.ConfigureEndpoints();

app.Run();

public record LoginRequest(
    [property: DefaultValue("carlfearby@me.com"), SwaggerSchema] string Email,
    [property: DefaultValue("password1234!"), SwaggerSchema] string Password
);