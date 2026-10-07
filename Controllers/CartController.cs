using System.Text.Json;
using EcommerceStore.Data;
using EcommerceStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace EcommerceStore.Controllers;

public class CartController : Controller
{
    private const string CartKey = "cart";
    private readonly AppDbContext _db;
    public CartController(AppDbContext db) => _db = db;

    // Cart is stored in the session as { productId: quantity }. Prices always come from the database.
    private Dictionary<int, int> GetCart()
    {
        var json = HttpContext.Session.GetString(CartKey);
        return json == null ? new() : JsonSerializer.Deserialize<Dictionary<int, int>>(json) ?? new();
    }
    private void SaveCart(Dictionary<int, int> cart) =>
        HttpContext.Session.SetString(CartKey, JsonSerializer.Serialize(cart));

    public async Task<IActionResult> Index()
    {
        var cart = GetCart();
        var ids = cart.Keys.ToList();
        var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        var rows = products.Select(p => new CartRow(p.Id, p.Name, p.Price, cart[p.Id])).ToList();
        return View(new CartViewModel(rows));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Add(int id)
    {
        var product = await _db.Products.FindAsync(id);
        if (product == null || product.Stock < 1) return NotFound();
        var cart = GetCart();
        cart.TryGetValue(id, out var qty);
        cart[id] = Math.Min(qty + 1, product.Stock);
        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ValidateAntiForgeryToken]
    public IActionResult Remove(int id)
    {
        var cart = GetCart();
        cart.Remove(id);
        SaveCart(cart);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Checkout() =>
        GetCart().Count == 0 ? RedirectToAction(nameof(Index)) : View(new Order());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout([Bind("CustomerName,Email,Address")] Order order)
    {
        var cart = GetCart();
        if (cart.Count == 0) return RedirectToAction(nameof(Index));
        if (!ModelState.IsValid) return View(order);

        var ids = cart.Keys.ToList();
        var products = await _db.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        foreach (var p in products)
        {
            var qty = cart[p.Id];
            if (qty > p.Stock)
            {
                ModelState.AddModelError("", $"Only {p.Stock} of {p.Name} left in stock.");
                return View(order);
            }
            order.Items.Add(new OrderItem { ProductId = p.Id, ProductName = p.Name, UnitPrice = p.Price, Quantity = qty });
            p.Stock -= qty;
        }
        order.Total = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        _db.Orders.Add(order);
        await _db.SaveChangesAsync();

        SaveCart(new());
        TempData["OrderId"] = order.Id;
        return RedirectToAction(nameof(Done));
    }

    public async Task<IActionResult> Done()
    {
        if (TempData.Peek("OrderId") is not int id) return RedirectToAction("Index", "Products");
        var order = await _db.Orders.FindAsync(id);
        return order == null ? NotFound() : View(order);
    }
}
