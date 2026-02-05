using System.Globalization;

namespace TwentyFourFood.Models;

public class RecipeIngredient
{
    public int? IngredientId { get; set; }

    public string Name { get; set; } = string.Empty;

    public double Quantity { get; set; }

    public string Unit { get; set; } = string.Empty;

    public bool HasQuantity => Quantity > 0 || !string.IsNullOrWhiteSpace(Unit);

    public string QuantityDisplay => HasQuantity
        ? string.Format(CultureInfo.CurrentCulture, "{0:0.##} {1}", Quantity, Unit).Trim()
        : string.Empty;
}
