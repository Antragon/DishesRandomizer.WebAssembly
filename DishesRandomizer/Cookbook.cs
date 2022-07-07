namespace DishesRandomizer.Common;

using System.Text.Json.Serialization;

[Serializable]
public class Cookbook {
    [JsonConstructor]
    public Cookbook(HashSet<Dish> dishes, Dictionary<Day, Dish> plannedDishes) {
        Dishes = dishes;
        PlannedDishes = plannedDishes;
    }

    public HashSet<Dish> Dishes { get; }
    public Dictionary<Day, Dish> PlannedDishes { get; }

    public static Cookbook Default { get; } = new(new HashSet<Dish>(), new Dictionary<Day, Dish>()) {
        Dishes = {
            new Dish(Guid.Parse("00000000-0000-0000-0000-000000000001"), "Burger"),
            new Dish(Guid.Parse("00000000-0000-0000-0000-000000000002"), "Pizza"),
            new Dish(Guid.Parse("00000000-0000-0000-0000-000000000003"), "Mashed Potatoes"),
        }
    };
}