using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TwentyFourFood.Models;
using TwentyFourFood.Resources.Strings;
using TwentyFourFood.Services;

namespace TwentyFourFood.ViewModels;

public class RecipesViewModel : INotifyPropertyChanged
{
    private readonly LiteDbService _liteDbService;

    private string _name = string.Empty;
    private string _category = string.Empty;
    private string _servingsText = string.Empty;
    private string _notes = string.Empty;
    private string _statusMessage = string.Empty;
    private Ingredient? _selectedIngredient;
    private string _ingredientName = string.Empty;
    private string _ingredientQuantityText = string.Empty;
    private string _ingredientUnit = string.Empty;

    public RecipesViewModel(LiteDbService liteDbService)
    {
        _liteDbService = liteDbService;

        AddRecipeCommand = new Command(AddRecipe);
        DeleteRecipeCommand = new Command<Recipe>(DeleteRecipe);
        AddRecipeIngredientCommand = new Command(AddRecipeIngredient);
        RemoveRecipeIngredientCommand = new Command<RecipeIngredient>(RemoveRecipeIngredient);

        LoadRecipes();
        LoadIngredients();
    }

    public ObservableCollection<Recipe> Recipes { get; } = new();

    public ObservableCollection<RecipeIngredient> RecipeIngredients { get; } = new();

    public ObservableCollection<Ingredient> AvailableIngredients { get; } = new();

    public string Name
    {
        get => _name;
        set => SetProperty(ref _name, value);
    }

    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public string ServingsText
    {
        get => _servingsText;
        set => SetProperty(ref _servingsText, value);
    }

    public string Notes
    {
        get => _notes;
        set => SetProperty(ref _notes, value);
    }

    public string StatusMessage
    {
        get => _statusMessage;
        set => SetProperty(ref _statusMessage, value);
    }

    public Ingredient? SelectedIngredient
    {
        get => _selectedIngredient;
        set
        {
            if (SetProperty(ref _selectedIngredient, value))
            {
                if (value is not null)
                {
                    if (string.IsNullOrWhiteSpace(IngredientName))
                    {
                        IngredientName = value.Name;
                    }

                    if (string.IsNullOrWhiteSpace(IngredientUnit))
                    {
                        IngredientUnit = value.Unit;
                    }
                }
            }
        }
    }

    public string IngredientName
    {
        get => _ingredientName;
        set => SetProperty(ref _ingredientName, value);
    }

    public string IngredientQuantityText
    {
        get => _ingredientQuantityText;
        set => SetProperty(ref _ingredientQuantityText, value);
    }

    public string IngredientUnit
    {
        get => _ingredientUnit;
        set => SetProperty(ref _ingredientUnit, value);
    }

    public ICommand AddRecipeCommand { get; }

    public ICommand DeleteRecipeCommand { get; }

    public ICommand AddRecipeIngredientCommand { get; }

    public ICommand RemoveRecipeIngredientCommand { get; }

    private void LoadRecipes()
    {
        Recipes.Clear();

        foreach (var recipe in _liteDbService.GetRecipes())
        {
            Recipes.Add(recipe);
        }
    }

    private void LoadIngredients()
    {
        AvailableIngredients.Clear();

        foreach (var ingredient in _liteDbService.GetIngredients())
        {
            AvailableIngredients.Add(ingredient);
        }
    }

    private void AddRecipeIngredient()
    {
        StatusMessage = string.Empty;

        var name = string.IsNullOrWhiteSpace(IngredientName)
            ? SelectedIngredient?.Name?.Trim() ?? string.Empty
            : IngredientName.Trim();

        if (string.IsNullOrWhiteSpace(name))
        {
            StatusMessage = AppResources.StatusIngredientNameRequired;
            return;
        }

        double quantityValue = 0;
        if (!string.IsNullOrWhiteSpace(IngredientQuantityText))
        {
            if (!double.TryParse(IngredientQuantityText, NumberStyles.Float, CultureInfo.CurrentCulture, out quantityValue))
            {
                StatusMessage = AppResources.StatusQuantityNumber;
                return;
            }
        }

        var unit = string.IsNullOrWhiteSpace(IngredientUnit)
            ? SelectedIngredient?.Unit?.Trim() ?? string.Empty
            : IngredientUnit.Trim();

        RecipeIngredients.Add(new RecipeIngredient
        {
            IngredientId = SelectedIngredient?.Id,
            Name = name,
            Quantity = quantityValue,
            Unit = unit
        });

        SelectedIngredient = null;
        IngredientName = string.Empty;
        IngredientQuantityText = string.Empty;
        IngredientUnit = string.Empty;
    }

    private void AddRecipe()
    {
        StatusMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            StatusMessage = AppResources.StatusRecipeNameRequired;
            return;
        }

        int servingsValue = 0;
        if (!string.IsNullOrWhiteSpace(ServingsText))
        {
            if (!int.TryParse(ServingsText, NumberStyles.Integer, CultureInfo.CurrentCulture, out servingsValue))
            {
                StatusMessage = AppResources.StatusServingsNumber;
                return;
            }
        }

        var recipe = new Recipe
        {
            Name = Name.Trim(),
            Category = Category.Trim(),
            Servings = servingsValue,
            Notes = Notes.Trim(),
            Ingredients = RecipeIngredients.ToList(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        _liteDbService.AddRecipe(recipe);
        Recipes.Insert(0, recipe);

        Name = string.Empty;
        Category = string.Empty;
        ServingsText = string.Empty;
        Notes = string.Empty;
        RecipeIngredients.Clear();

        StatusMessage = AppResources.StatusRecipeSaved;
    }

    private void DeleteRecipe(Recipe? recipe)
    {
        if (recipe is null)
        {
            return;
        }

        _liteDbService.DeleteRecipe(recipe.Id);
        Recipes.Remove(recipe);
    }

    private void RemoveRecipeIngredient(RecipeIngredient? ingredient)
    {
        if (ingredient is null)
        {
            return;
        }

        RecipeIngredients.Remove(ingredient);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return false;
        }

        storage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        return true;
    }
}
