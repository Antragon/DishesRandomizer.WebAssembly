namespace DishesRandomizer.Common;

using System.Collections.ObjectModel;

public class PlannedDishes : ReadOnlyDictionary<Day, Dish> {
    public static PlannedDishes Default => new(new Dictionary<Day, Dish> {
        [Day.Monday] = SomeDish.Pizza,
        [Day.Tuesday] = SomeDish.Pizza,
        [Day.Wednesday] = SomeDish.Pizza,
        [Day.Thursday] = SomeDish.Pizza,
        [Day.Friday] = SomeDish.Pizza,
        [Day.Saturday] = SomeDish.Pizza,
        [Day.Sunday] = SomeDish.Pizza,
    });

    public PlannedDishes()
        : base(new Dictionary<Day, Dish>()) { }

    public PlannedDishes(IDictionary<Day, Dish> dictionary)
        : base(dictionary) { }
}