namespace DishesRandomizer.WebAssembly.Models;

public static class SomeDish {
    public static Dish Pizza { get; } = new(Guid.NewGuid(), "Pizza");
    public static Dish Burger { get; } = new(Guid.NewGuid(), "Burger");
    public static Dish MashedPotatoes { get; } = new(Guid.NewGuid(), "Mashed Potatoes");
}