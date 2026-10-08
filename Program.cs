using EcommerceStore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Vercel PORT support
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var provider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";

builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (provider == "SqlServer")
    {
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("SqlServer"));
    }
    else
    {
        options.UseSqlite(
            builder.Configuration.GetConnectionString("Sqlite"));
    }
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.EnsureCreated();
}

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Products}/{action=Index}/{id?}");

app.Run();