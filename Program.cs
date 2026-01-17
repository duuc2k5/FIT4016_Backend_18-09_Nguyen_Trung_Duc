using Microsoft.EntityFrameworkCore;
using OrderManagementApp.Models;
using OrderManagementApp.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? "Server=DUCK;Database=OrderManagement;Trusted_Connection=true;";

builder.Services.AddDbContext<OrderManagementContext>(options =>
    options.UseSqlServer(connectionString));

var app = builder.Build();

// Apply migrations and seed data (wrapped in try-catch to not block app startup)
_ = Task.Run(async () =>
{
    try
    {
        await Task.Delay(1000); // Wait for app to start
        using (var scope = app.Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<OrderManagementContext>();
            await context.Database.MigrateAsync();
            SeedData.Initialize(context);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Database initialization error: {ex.Message}");
    }
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
