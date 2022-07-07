namespace DishesRandomizer.WebAssembly;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Common;
using MoreLinq.Extensions;

public class CookbookController {
    private readonly Subject<Cookbook> _cookbookSubject = new();
    private readonly Cookbook _cookbook;

    public CookbookController(Cookbook cookbook) {
        _cookbook = cookbook;
    }

    public IObservable<Cookbook> CookbookChanged => _cookbookSubject.AsObservable();

    public void SetDish(Dish dish) {
        _cookbook.Dishes.Add(dish);
        _cookbookSubject.OnNext(_cookbook);
    }

    public void DeleteDish(Dish dish) {
        _cookbook.Dishes.Remove(dish);
        _cookbookSubject.OnNext(_cookbook);
    }

    public IEnumerable<Dish> GetDishes() {
        return _cookbook.Dishes;
    }

    public void SetPlannedDish(Day day, Dish dish) {
        _cookbook.PlannedDishes[day] = dish;
        _cookbookSubject.OnNext(_cookbook);
    }

    public void DeletePlannedDish(Day day) {
        _cookbook.PlannedDishes.Remove(day);
        _cookbookSubject.OnNext(_cookbook);
    }

    public Dish? GetPlannedDish(Day day) {
        return _cookbook.PlannedDishes.TryGetValue(day, out var dish) ? dish : null;
    }

    public void ShufflePlannedDishes(params Day[] days) {
        var otherDays = Enum.GetValues<Day>().Except(days).ToList();
        var alreadyPlanned = otherDays.Select(GetPlannedDish).OfType<Dish>();
        var unplannedDishes = _cookbook.Dishes.Except(alreadyPlanned).ToList();
        foreach (var day in days) {
            var dishesSelection = unplannedDishes.Any() ? unplannedDishes : _cookbook.Dishes.ToList();
            var selectedDish = dishesSelection.RandomSubset(1).Single();
            _cookbook.PlannedDishes[day] = selectedDish;
            unplannedDishes.Remove(selectedDish);
        }

        _cookbookSubject.OnNext(_cookbook);
    }
}