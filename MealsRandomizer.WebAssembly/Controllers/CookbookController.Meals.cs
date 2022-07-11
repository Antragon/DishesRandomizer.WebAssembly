namespace MealsRandomizer.WebAssembly.Controllers;

using MoreLinq;

public partial class CookbookController {
    public void AddMeal(Meal meal) {
        Cookbook.Meals.Add(meal.Id, meal);
        _cookbookSubject.OnNext(Cookbook);
    }

    public void SetMealName(Guid id, string name) {
        Cookbook.Meals[id].Name = name;
        _cookbookSubject.OnNext(Cookbook);
    }

    public void DeleteMeal(Guid id) {
        Cookbook.Meals.Remove(id);
        var days = Cookbook.PlannedMeals.Where(x => x.Value == id).Select(x => x.Key);
        days.ForEach(day => Cookbook.PlannedMeals.Remove(day));
        _cookbookSubject.OnNext(Cookbook);
    }

    public IEnumerable<Meal> GetMeals() {
        return Cookbook.Meals.Values;
    }

    public void SetPlannedMeal(Day day, Guid id) {
        Cookbook.PlannedMeals[day] = id;
        _cookbookSubject.OnNext(Cookbook);
    }

    public void DeletePlannedMeal(Day day) {
        Cookbook.PlannedMeals.Remove(day);
        _cookbookSubject.OnNext(Cookbook);
    }

    public Meal? GetPlannedMeal(Day day) {
        return Cookbook.PlannedMeals.TryGetValue(day, out var id) ? Cookbook.Meals[id] : null;
    }

    public void ShufflePlannedMeals(params Day[] days) {
        var otherDays = Enum.GetValues<Day>().Except(days).ToList();
        var alreadyPlanned = otherDays.Select(GetPlannedMeal).OfType<Meal>();
        var unplannedMeals = Cookbook.Meals.Values.Except(alreadyPlanned).ToList();
        foreach (var day in days) {
            var mealsSelection = unplannedMeals.Any() ? unplannedMeals : Cookbook.Meals.Values.ToList();
            var selectedMeal = mealsSelection.RandomSubset(1).Single();
            Cookbook.PlannedMeals[day] = selectedMeal.Id;
            unplannedMeals.Remove(selectedMeal);
        }

        _cookbookSubject.OnNext(Cookbook);
    }
}