using LiteDB;
using TwentyFourFood.Models;

namespace TwentyFourFood.Services;

public class LiteDbService
{
    private readonly string _databasePath;

    public LiteDbService()
    {
        var folder = FileSystem.AppDataDirectory;
        _databasePath = Path.Combine(folder, "ingredients.db");
    }

    private LiteDatabase OpenDatabase()
    {
        var connectionString = new ConnectionString
        {
            Filename = _databasePath,
            Connection = ConnectionType.Shared
        };

        return new LiteDatabase(connectionString);
    }

    public List<Ingredient> GetIngredients()
    {
        using var db = OpenDatabase();
        var collection = db.GetCollection<Ingredient>("ingredients");

        return collection.Query()
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }

    public Ingredient AddIngredient(Ingredient ingredient)
    {
        using var db = OpenDatabase();
        var collection = db.GetCollection<Ingredient>("ingredients");

        collection.Insert(ingredient);

        return ingredient;
    }

    public void DeleteIngredient(int id)
    {
        using var db = OpenDatabase();
        var collection = db.GetCollection<Ingredient>("ingredients");

        collection.Delete(id);
    }

    public List<Recipe> GetRecipes()
    {
        using var db = OpenDatabase();
        var collection = db.GetCollection<Recipe>("recipes");

        return collection.Query()
            .OrderByDescending(x => x.CreatedAt)
            .ToList();
    }

    public Recipe AddRecipe(Recipe recipe)
    {
        using var db = OpenDatabase();
        var collection = db.GetCollection<Recipe>("recipes");

        collection.Insert(recipe);

        return recipe;
    }

    public void DeleteRecipe(int id)
    {
        using var db = OpenDatabase();
        var collection = db.GetCollection<Recipe>("recipes");

        collection.Delete(id);
    }
}
