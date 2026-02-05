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

    public RecipesViewModel(LiteDbService liteDbService)
    {
        _liteDbService = liteDbService;

        AddRecipeCommand = new Command(AddRecipe);
        DeleteRecipeCommand = new Command<Recipe>(DeleteRecipe);

        LoadRecipes();
    }

    public ObservableCollection<Recipe> Recipes { get; } = new();

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

    public ICommand AddRecipeCommand { get; }

    public ICommand DeleteRecipeCommand { get; }

    private void LoadRecipes()
    {
        Recipes.Clear();

        foreach (var recipe in _liteDbService.GetRecipes())
        {
            Recipes.Add(recipe);
        }
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
            CreatedAt = DateTimeOffset.UtcNow
        };

        _liteDbService.AddRecipe(recipe);
        Recipes.Insert(0, recipe);

        Name = string.Empty;
        Category = string.Empty;
        ServingsText = string.Empty;
        Notes = string.Empty;

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

    public event PropertyChangedEventHandler? PropertyChanged;

    private void SetProperty<T>(ref T storage, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(storage, value))
        {
            return;
        }

        storage = value;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
