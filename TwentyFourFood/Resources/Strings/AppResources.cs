using System.Globalization;
using System.Resources;

namespace TwentyFourFood.Resources.Strings;

public static class AppResources
{
    private static readonly ResourceManager ResourceManager =
        new("TwentyFourFood.Resources.Strings.AppResources", typeof(AppResources).Assembly);

    private static string GetString(string name)
    {
        return ResourceManager.GetString(name, CultureInfo.CurrentUICulture) ?? $"[[{name}]]";
    }

    public static string IngredientsTitle => GetString(nameof(IngredientsTitle));
    public static string IngredientsSubtitle => GetString(nameof(IngredientsSubtitle));
    public static string NewIngredientTitle => GetString(nameof(NewIngredientTitle));
    public static string NameLabel => GetString(nameof(NameLabel));
    public static string NamePlaceholder => GetString(nameof(NamePlaceholder));
    public static string CategoryLabel => GetString(nameof(CategoryLabel));
    public static string CategoryPlaceholder => GetString(nameof(CategoryPlaceholder));
    public static string QuantityLabel => GetString(nameof(QuantityLabel));
    public static string QuantityPlaceholder => GetString(nameof(QuantityPlaceholder));
    public static string UnitLabel => GetString(nameof(UnitLabel));
    public static string UnitPlaceholder => GetString(nameof(UnitPlaceholder));
    public static string NotesLabel => GetString(nameof(NotesLabel));
    public static string NotesPlaceholder => GetString(nameof(NotesPlaceholder));
    public static string AddIngredientButton => GetString(nameof(AddIngredientButton));
    public static string DeleteButton => GetString(nameof(DeleteButton));
    public static string EmptyTitle => GetString(nameof(EmptyTitle));
    public static string EmptySubtitle => GetString(nameof(EmptySubtitle));
    public static string HomeTitle => GetString(nameof(HomeTitle));
    public static string StatusNameRequired => GetString(nameof(StatusNameRequired));
    public static string StatusQuantityNumber => GetString(nameof(StatusQuantityNumber));
    public static string StatusIngredientSaved => GetString(nameof(StatusIngredientSaved));
}
