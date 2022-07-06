namespace DishesRandomizer.Common;

using MoreLinq;

public class PlannedDishes : Dictionary<Day, Dish> {
    private readonly Dishes _dishes;

    public PlannedDishes(Dishes dishes)
        : base(new Dictionary<Day, Dish>()) {
        _dishes = dishes;
    }

    public PlannedDishes(IDictionary<Day, Dish> dictionary, Dishes dishes)
        : base(dictionary) {
        _dishes = dishes;
    }

    public void Shuffle(params Day[] days) {
        var unplannedDishes = _dishes.Except(Values).ToList();
        foreach (var day in days) {
            if (unplannedDishes.Any()) {
                var selectedDish = unplannedDishes.RandomSubset(1).Single();
                this[day] = selectedDish;
                unplannedDishes.Remove(selectedDish);
            } else {
                Remove(day);
            }
        }
    }
}