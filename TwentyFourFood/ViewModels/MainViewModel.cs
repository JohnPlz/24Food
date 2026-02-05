using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TwentyFourFood.Models;
using TwentyFourFood.Resources.Strings;
using TwentyFourFood.Services;

namespace TwentyFourFood.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly LiteDbService _liteDbService;

    private string _name = string.Empty;
    private string _category = string.Empty;
    private string _quantityText = string.Empty;
    private string _unit = string.Empty;
    private string _notes = string.Empty;
    private string _statusMessage = string.Empty;

    public MainViewModel(LiteDbService liteDbService)
    {
        _liteDbService = liteDbService;

        AddIngredientCommand = new Command(AddIngredient);
        DeleteIngredientCommand = new Command<Ingredient>(DeleteIngredient);

        LoadIngredients();
    }

    public ObservableCollection<Ingredient> Ingredients { get; } = new();

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

    public string QuantityText
    {
        get => _quantityText;
        set => SetProperty(ref _quantityText, value);
    }

    public string Unit
    {
        get => _unit;
        set => SetProperty(ref _unit, value);
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

    public ICommand AddIngredientCommand { get; }

    public ICommand DeleteIngredientCommand { get; }

    private void LoadIngredients()
    {
        Ingredients.Clear();

        foreach (var ingredient in _liteDbService.GetIngredients())
        {
            Ingredients.Add(ingredient);
        }
    }

    private void AddIngredient()
    {
        StatusMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Name))
        {
            StatusMessage = AppResources.StatusNameRequired;
            return;
        }

        double quantityValue = 0;
        if (!string.IsNullOrWhiteSpace(QuantityText))
        {
            if (!double.TryParse(QuantityText, NumberStyles.Float, CultureInfo.CurrentCulture, out quantityValue))
            {
                StatusMessage = AppResources.StatusQuantityNumber;
                return;
            }
        }

        var ingredient = new Ingredient
        {
            Name = Name.Trim(),
            Category = Category.Trim(),
            Quantity = quantityValue,
            Unit = Unit.Trim(),
            Notes = Notes.Trim(),
            CreatedAt = DateTimeOffset.UtcNow
        };

        _liteDbService.AddIngredient(ingredient);
        Ingredients.Insert(0, ingredient);

        Name = string.Empty;
        Category = string.Empty;
        QuantityText = string.Empty;
        Unit = string.Empty;
        Notes = string.Empty;

        StatusMessage = AppResources.StatusIngredientSaved;
    }

    private void DeleteIngredient(Ingredient? ingredient)
    {
        if (ingredient is null)
        {
            return;
        }

        _liteDbService.DeleteIngredient(ingredient.Id);
        Ingredients.Remove(ingredient);
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
