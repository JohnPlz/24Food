using System.Globalization;
using LiteDB;
using TwentyFourFood.Resources.Strings;

namespace TwentyFourFood.Models;

public class Recipe
{
    [BsonId(true)]
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public int Servings { get; set; }

    public string Notes { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public bool HasCategory => !string.IsNullOrWhiteSpace(Category);

    public bool HasNotes => !string.IsNullOrWhiteSpace(Notes);

    public bool HasServings => Servings > 0;

    public string ServingsDisplay => HasServings
        ? string.Format(CultureInfo.CurrentCulture, AppResources.ServingsTagFormat, Servings)
        : string.Empty;

    public string CreatedAtDisplay => CreatedAt.ToLocalTime().ToString("g", CultureInfo.CurrentCulture);
}
