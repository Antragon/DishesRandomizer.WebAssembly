namespace MealsRandomizer.WebAssembly;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using MoreLinq;

public class CookbookController {
    private Cookbook _cookbook;
    private readonly Subject<Cookbook> _cookbookSubject = new();

    public CookbookController(Cookbook cookbook) {
        _cookbook = cookbook;
    }

    public Cookbook Cookbook {
        get => _cookbook;
        set => SetCookbook(value);
    }

    private void SetCookbook(Cookbook value) {
        _cookbook = value;
        _cookbookSubject.OnNext(Cookbook);
    }

    public IObservable<Cookbook> CookbookChanged => _cookbookSubject.AsObservable();

    public void AddMeal(Dish meal) {
        Cookbook.Dishes.Add(meal.Id, meal);
        _cookbookSubject.OnNext(Cookbook);
    }

    public void SetMealName(Guid id, string name) {
        Cookbook.Dishes[id].Name = name;
        _cookbookSubject.OnNext(Cookbook);
    }

    public void DeleteMeal(Guid id) {
        Cookbook.Dishes.Remove(id);
        var days = Cookbook.PlannedDishes.Where(x => x.Value == id).Select(x => x.Key);
        days.ForEach(day => Cookbook.PlannedDishes.Remove(day));
        _cookbookSubject.OnNext(Cookbook);
    }

    public IEnumerable<Dish> GetMeals() {
        return Cookbook.Dishes.Values;
    }

    public void SetPlannedMeal(Day day, Guid id) {
        Cookbook.PlannedDishes[day] = id;
        _cookbookSubject.OnNext(Cookbook);
    }

    public void DeletePlannedMeal(Day day) {
        Cookbook.PlannedDishes.Remove(day);
        _cookbookSubject.OnNext(Cookbook);
    }

    public Dish? GetPlannedMeal(Day day) {
        return Cookbook.PlannedDishes.TryGetValue(day, out var id) ? Cookbook.Dishes[id] : null;
    }

    public void ShufflePlannedMeals(params Day[] days) {
        var otherDays = Enum.GetValues<Day>().Except(days).ToList();
        var alreadyPlanned = otherDays.Select(GetPlannedMeal).OfType<Dish>();
        var unplannedDishes = Cookbook.Dishes.Values.Except(alreadyPlanned).ToList();
        foreach (var day in days) {
            var dishesSelection = unplannedDishes.Any() ? unplannedDishes : Cookbook.Dishes.Values.ToList();
            var selectedDish = dishesSelection.RandomSubset(1).Single();
            Cookbook.PlannedDishes[day] = selectedDish.Id;
            unplannedDishes.Remove(selectedDish);
        }

        _cookbookSubject.OnNext(Cookbook);
    }
}