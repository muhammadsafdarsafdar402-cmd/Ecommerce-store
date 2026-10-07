namespace EcommerceStore.Models;

public record CartRow(int ProductId, string Name, decimal Price, int Quantity);

public class CartViewModel
{
    public List<CartRow> Rows { get; }
    public decimal Total => Rows.Sum(r => r.Price * r.Quantity);
    public CartViewModel(List<CartRow> rows) => Rows = rows;
}
