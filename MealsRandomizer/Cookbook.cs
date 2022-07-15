namespace MealsRandomizer;

using System.Text.Json.Serialization;

[Serializable]
public class Cookbook {
    [JsonConstructor]
    public Cookbook(Dictionary<Guid, Ingredient>? ingredients, Dictionary<Guid, Meal>? meals, Dictionary<Day, Guid>? plannedMeals) {
        Ingredients = ingredients ?? new Dictionary<Guid, Ingredient>();
        Meals = meals ?? new Dictionary<Guid, Meal>();
        PlannedMeals = plannedMeals ?? new Dictionary<Day, Guid>();
    }

    public Dictionary<Guid, Ingredient> Ingredients { get; }
    public Dictionary<Guid, Meal> Meals { get; }
    public Dictionary<Day, Guid> PlannedMeals { get; }

    public static Cookbook Default { get; } = new(
        new Dictionary<Guid, Ingredient>(),
        new Dictionary<Guid, Meal>(),
        new Dictionary<Day, Guid>()) {
        Meals = {
            [Convert(1)] = new Meal(Convert(1), "Burger", null),
            [Convert(2)] = new Meal(Convert(2), "Pizza", null),
            [Convert(3)] = new Meal(Convert(3), "Mashed Potatoes", null),
        }
    };

    private static Guid Convert(int number) {
        var bytes = new byte[16];
        BitConverter.GetBytes(number).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}