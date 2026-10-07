using EcommerceStore.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

// "Sqlite" works everywhere with no setup. Change DatabaseProvider to "SqlServer" in appsettings.json to use SQL Server.
var provider = builder.Configuration["DatabaseProvider"] ?? "Sqlite";
builder.Services.AddDbContext<AppDbContext>(o =>
{
    if (provider == "SqlServer")
        o.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer"));
    else
        o.UseSqlite(builder.Configuration.GetConnectionString("Sqlite"));
});

var app = builder.Build();

// Creates the database and seeds sample products on first run.
using (var scope = app.Services.CreateScope())
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
app.MapControllerRoute("default", "{controller=Products}/{action=Index}/{id?}");
app.Run();
