namespace MealsRandomizer.WebAssembly.Pages;

using Controllers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Meals {
    private const int _pageSize = 7;
    
    private readonly List<Meal> _mealsSelection = new();
    private readonly List<RadzenTextBox> _textBoxes = new();

    private RadzenPager? _pager;
    private int _count;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private RadzenTextBox TextBoxRef {
        set => _textBoxes.Add(value);
    }

    protected override void OnInitialized() {
        SetMealsSelection(0, _pageSize);
        _count = CookbookController.GetMeals().Count();
    }

    private async Task AddMeal() {
        var meal = new Meal(Guid.NewGuid(), string.Empty, null);
        CookbookController.AddMeal(meal);
        _mealsSelection.Insert(0, meal);
        if (_mealsSelection.Count > _pageSize) {
            _mealsSelection.RemoveAt(_pageSize);
        }

        _count++;
        if (_pager != null) {
            await _pager.FirstPage();
        }

        var textBox = _textBoxes.FirstOrDefault();
        if (textBox != null) {
            await textBox.Element.FocusAsync();
        }
    }

    private Task DeleteMeal(Meal meal) {
        CookbookController.DeleteMeal(meal.Id);
        _mealsSelection.Remove(meal);
        _count--;
        return _pager?.GoToPage(_pager.CurrentPage, true) ?? Task.CompletedTask;
    }

    private void MealChanged(Guid id, string name) {
        CookbookController.SetMealName(id, name);
    }

    private void PageChanged(PagerEventArgs args) {
        var skip = args.Skip;
        var take = args.Top;
        SetMealsSelection(skip, take);
    }

    private void SetMealsSelection(int skip, int take) {
        var meals = CookbookController.GetMeals().OrderBy(meal => meal);
        _mealsSelection.Clear();
        _mealsSelection.AddRange(meals.Skip(skip).Take(take));
    }
}