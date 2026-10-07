using EcommerceStore.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EcommerceStore.Controllers;

public class ProductsController : Controller
{
    private readonly AppDbContext _db;
    public ProductsController(AppDbContext db) => _db = db;

    public async Task<IActionResult> Index(string? q, string? category)
    {
        var query = _db.Products.AsQueryable();
        if (!string.IsNullOrWhiteSpace(q))
            query = query.Where(p => p.Name.Contains(q) || p.Description.Contains(q));
        if (!string.IsNullOrWhiteSpace(category))
            query = query.Where(p => p.Category == category);

        ViewBag.Q = q;
        ViewBag.Category = category;
        ViewBag.Categories = await _db.Products.Select(p => p.Category).Distinct().OrderBy(c => c).ToListAsync();
        return View(await query.OrderBy(p => p.Name).ToListAsync());
    }

    public async Task<IActionResult> Details(int id)
    {
        var product = await _db.Products.FindAsync(id);
        return product == null ? NotFound() : View(product);
    }
}
