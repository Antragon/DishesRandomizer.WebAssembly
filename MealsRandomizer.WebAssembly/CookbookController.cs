namespace MealsRandomizer.WebAssembly;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using MoreLinq;

public class CookbookController {
    private readonly Subject<Cookbook> _cookbookSubject = new();
    private readonly Cookbook _cookbook;

    public CookbookController(Cookbook cookbook) {
        _cookbook = cookbook;
    }

    public IObservable<Cookbook> CookbookChanged => _cookbookSubject.AsObservable();

    public void AddMeal(Dish meal) {
        _cookbook.Dishes.Add(meal.Id, meal);
        _cookbookSubject.OnNext(_cookbook);
    }

    public void SetMealName(Guid id, string name) {
        _cookbook.Dishes[id].Name = name;
        _cookbookSubject.OnNext(_cookbook);
    }

    public void DeleteMeal(Guid id) {
        _cookbook.Dishes.Remove(id);
        var days = _cookbook.PlannedDishes.Where(x => x.Value == id).Select(x => x.Key);
        days.ForEach(day => _cookbook.PlannedDishes.Remove(day));
        _cookbookSubject.OnNext(_cookbook);
    }

    public IEnumerable<Dish> GetMeals() {
        return _cookbook.Dishes.Values;
    }

    public void SetPlannedMeal(Day day, Guid id) {
        _cookbook.PlannedDishes[day] = id;
        _cookbookSubject.OnNext(_cookbook);
    }

    public void DeletePlannedMeal(Day day) {
        _cookbook.PlannedDishes.Remove(day);
        _cookbookSubject.OnNext(_cookbook);
    }

    public Dish? GetPlannedMeal(Day day) {
        return _cookbook.PlannedDishes.TryGetValue(day, out var id) ? _cookbook.Dishes[id] : null;
    }

    public void ShufflePlannedMeals(params Day[] days) {
        var otherDays = Enum.GetValues<Day>().Except(days).ToList();
        var alreadyPlanned = otherDays.Select(GetPlannedMeal).OfType<Dish>();
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