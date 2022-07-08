namespace MealsRandomizer;

using System.Text.Json.Serialization;

[Serializable]
public class Cookbook {
    [JsonConstructor]
    public Cookbook(Dictionary<Guid, Dish> dishes, Dictionary<Day, Guid> plannedDishes) {
        Dishes = dishes;
        PlannedDishes = plannedDishes;
    }

    public Dictionary<Guid, Dish> Dishes { get; }
    public Dictionary<Day, Guid> PlannedDishes { get; }

    public static Cookbook Default { get; } = new(new Dictionary<Guid, Dish>(), new Dictionary<Day, Guid>()) {
        Dishes = {
            [Convert(1)] = new Dish(Convert(1), "Burger"),
            [Convert(2)] = new Dish(Convert(2), "Pizza"),
            [Convert(3)] = new Dish(Convert(3), "Mashed Potatoes"),
        }
    };

    private static Guid Convert(int number) {
        var bytes = new byte[16];
        BitConverter.GetBytes(number).CopyTo(bytes, 0);
        return new Guid(bytes);
    }
}