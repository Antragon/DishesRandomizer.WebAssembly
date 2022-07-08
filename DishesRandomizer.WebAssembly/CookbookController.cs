namespace DishesRandomizer.WebAssembly;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Common;
using MoreLinq;

public class CookbookController {
    private readonly Subject<Cookbook> _cookbookSubject = new();
    private readonly Cookbook _cookbook;

    public CookbookController(Cookbook cookbook) {
        _cookbook = cookbook;
    }

    public IObservable<Cookbook> CookbookChanged => _cookbookSubject.AsObservable();

    public void AddDish(Dish dish) {
        _cookbook.Dishes.Add(dish.Id, dish);
        _cookbookSubject.OnNext(_cookbook);
    }

    public void SetDishName(Guid id, string name) {
        _cookbook.Dishes[id].Name = name;
        _cookbookSubject.OnNext(_cookbook);
    }

    public void DeleteDish(Guid id) {
        _cookbook.Dishes.Remove(id);
        var days = _cookbook.PlannedDishes.Where(x => x.Value == id).Select(x => x.Key);
        days.ForEach(day => _cookbook.PlannedDishes.Remove(day));
        _cookbookSubject.OnNext(_cookbook);
    }

    public IEnumerable<Dish> GetDishes() {
        return _cookbook.Dishes.Values;
    }

    public void SetPlannedDish(Day day, Guid id) {
        _cookbook.PlannedDishes[day] = id;
        _cookbookSubject.OnNext(_cookbook);
    }

    public void DeletePlannedDish(Day day) {
        _cookbook.PlannedDishes.Remove(day);
        _cookbookSubject.OnNext(_cookbook);
    }

    public Dish? GetPlannedDish(Day day) {
        return _cookbook.PlannedDishes.TryGetValue(day, out var id) ? _cookbook.Dishes[id] : null;
    }

    public void ShufflePlannedDishes(params Day[] days) {
        var otherDays = Enum.GetValues<Day>().Except(days).ToList();
        var alreadyPlanned = otherDays.Select(GetPlannedDish).OfType<Dish>();
        var unplannedDishes = _cookbook.Dishes.Values.Except(alreadyPlanned).ToList();
        foreach (var day in days) {
            var dishesSelection = unplannedDishes.Any() ? unplannedDishes : _cookbook.Dishes.Values.ToList();
            var selectedDish = dishesSelection.RandomSubset(1).Single();
            _cookbook.PlannedDishes[day] = selectedDish.Id;
            unplannedDishes.Remove(selectedDish);
        }

        _cookbookSubject.OnNext(_cookbook);
    }
}