using System.ComponentModel.DataAnnotations;
namespace EcommerceStore.Models;

public class Product
{
    public int Id { get; set; }
    [Required, StringLength(100)] public string Name { get; set; } = "";
    [StringLength(500)] public string Description { get; set; } = "";
    [Range(0.01, 10000000)] public decimal Price { get; set; }
    [Required, StringLength(50)] public string Category { get; set; } = "";
    [Range(0, 100000)] public int Stock { get; set; }
}
