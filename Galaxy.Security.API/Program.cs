using Galaxy.Security.API.Middlewares;
using Galaxy.Security.Application;
using Galaxy.Security.Infraestructure;
using Galaxy.Security.Infraestructure.Configurations.Auth;
using Galaxy.Security.Infraestructure.Configurations.IdentitySeed;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddApplication();
builder.Services.AddInfraestructure(builder.Configuration);
builder.Services.AddJwtWithCookies(builder.Configuration);

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor",
        policy =>
        {
            policy
            .WithOrigins("http://localhost:5263")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials(); // This allows cookies to be sent
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandlingMiddleware();

app.UseHttpsRedirection();

app.UseMiddleware<RefreshTokenMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.UseCors("AllowBlazor");

//Exec Seeder
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await IdentityDataSeeder.SeedAsync(services);
}

app.Run();
