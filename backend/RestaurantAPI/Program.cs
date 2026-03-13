using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RestaurantAPI.Data;
using RestaurantAPI.Models;
using RestaurantAPI.Repositories;
using RestaurantAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwtKey = builder.Configuration["Jwt:Key"] ?? "YourSuperSecretKeyThatIsAtLeast32CharactersLong!";
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"] ?? "RestaurantAPI",
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"] ?? "RestaurantApp",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=restaurant.db";

// Use SQLite if connection string points to a file (no SQL Server required in Development)
builder.Services.AddDbContext<RestaurantDbContext>(options =>
{
    if (connectionString.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase))
        options.UseSqlite(connectionString);
    else
        options.UseSqlServer(connectionString);
    // Allow running existing migrations when model has minor changes (e.g. HasPrecision)
    options.ConfigureWarnings(w => w.Ignore(Microsoft.EntityFrameworkCore.Diagnostics.RelationalEventId.PendingModelChangesWarning));
});

builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.AddScoped<IMenuService, MenuService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// One-time: delete all entries from Orders, Users, Customers (schema unchanged)
if (args.Contains("--clear-data"))
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
    db.OrderItems.RemoveRange(db.OrderItems);
    db.Orders.RemoveRange(db.Orders);
    db.Users.RemoveRange(db.Users);
    db.Customers.RemoveRange(db.Customers);
    db.SaveChanges();
    Console.WriteLine("Cleared all entries from OrderItems, Orders, Users, and Customers.");
    return;
}

// Create or update database and seed data (Development only)
if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var db = scope.ServiceProvider.GetRequiredService<RestaurantDbContext>();
    var isSqlite = connectionString.TrimStart().StartsWith("Data Source=", StringComparison.OrdinalIgnoreCase);
    if (isSqlite)
    {
        db.Database.EnsureCreated(); // SQLite: create from model (no SQL Server migration)
        // Ensure Users table exists (for DBs created before User was added)
        db.Database.ExecuteSqlRaw(
            "CREATE TABLE IF NOT EXISTS \"Users\" (\"Id\" INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, \"Name\" TEXT NOT NULL, \"Email\" TEXT NOT NULL, \"PasswordHash\" TEXT NOT NULL, \"Role\" TEXT NOT NULL, \"CustomerId\" INTEGER NULL, FOREIGN KEY (\"CustomerId\") REFERENCES \"Customers\" (\"CustomerId\"))");
        db.Database.ExecuteSqlRaw("CREATE UNIQUE INDEX IF NOT EXISTS \"IX_Users_Email\" ON \"Users\" (\"Email\")");
    }
    else
        db.Database.Migrate(); // SQL Server: apply migrations
    if (!db.Customers.Any())
    {
        db.Customers.Add(new Customer { Name = "Guest Customer", Email = "guest@example.com", Phone = "555-0000" });
        db.SaveChanges();
    }
    if (!db.MenuItems.Any())
    {
        db.MenuItems.AddRange(
            new MenuItem { Name = "Margherita Pizza", Category = "Pizza", Price = 12.99m, IsAvailable = true },
            new MenuItem { Name = "Caesar Salad", Category = "Salad", Price = 8.99m, IsAvailable = true },
            new MenuItem { Name = "Burger & Fries", Category = "Main", Price = 11.99m, IsAvailable = true }
        );
        db.SaveChanges();
    }
    if (!db.Users.Any())
    {
        var adminHash = BCrypt.Net.BCrypt.HashPassword("Admin123!", BCrypt.Net.BCrypt.GenerateSalt(12));
        db.Users.Add(new User { Name = "Admin", Email = "admin@restaurant.com", PasswordHash = adminHash, Role = "Admin" });
        db.SaveChanges();
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
