using LiteDB;

namespace TwentyFourFood.Models;

public class Ingredient
{
    [BsonId(true)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public bool HasCategory => !string.IsNullOrWhiteSpace(Category);

    public bool HasNotes => !string.IsNullOrWhiteSpace(Notes);

    public bool HasQuantity => Quantity > 0 || !string.IsNullOrWhiteSpace(Unit);

    public string QuantityDisplay => HasQuantity ? $"{Quantity:0.##} {Unit}".Trim() : string.Empty;

    public string CreatedAtDisplay => CreatedAt.ToLocalTime().ToString("MMM d, yyyy h:mm tt");
}
