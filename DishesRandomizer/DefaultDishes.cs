namespace DishesRandomizer.Common;

using System.Collections.ObjectModel;

public class DefaultDishes : ReadOnlyDictionary<Day, Dish> {
    private DefaultDishes(IDictionary<Day, Dish> dictionary)
        : base(dictionary) { }

    public static DefaultDishes Create() {
        var defaults = new Dictionary<Day, Dish> {
            [Day.Monday] = SomeDish.Pizza,
            [Day.Tuesday] = SomeDish.Pizza,
            [Day.Wednesday] = SomeDish.Pizza,
            [Day.Thursday] = SomeDish.Pizza,
            [Day.Friday] = SomeDish.Pizza,
            [Day.Saturday] = SomeDish.Pizza,
            [Day.Sunday] = SomeDish.Pizza,
        };
        return new DefaultDishes(defaults);
    }
}