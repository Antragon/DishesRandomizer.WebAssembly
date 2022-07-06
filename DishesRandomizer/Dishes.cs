namespace DishesRandomizer.Common;

using System.Collections.ObjectModel;

public class Dishes : ReadOnlyCollection<Dish> {
    public static Dishes Default => new(new List<Dish> {
        SomeDish.Burger,
        SomeDish.Pizza,
        SomeDish.MashedPotatoes,
    });

    public Dishes()
        : base(new List<Dish>()) { }

    public Dishes(IList<Dish> list)
        : base(list) { }
}