namespace MealsRandomizer.WebAssembly.Pages;

using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Meals {
    private readonly List<Dish> _dishesSelection = new();
    private readonly List<RadzenTextBox> _textBoxes = new();
    private readonly int _pageSize = 7;

    private RadzenPager? _pager;
    private int _count;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private RadzenTextBox TextBoxRef {
        set => _textBoxes.Add(value);
    }

    protected override void OnInitialized() {
        SetDishesSelection(0, _pageSize);
        _count = CookbookController.GetMeals().Count();
    }

    private async Task AddDish() {
        var dish = new Dish(Guid.NewGuid(), string.Empty);
        CookbookController.AddMeal(dish);
        _dishesSelection.Insert(0, dish);
        if (_dishesSelection.Count > _pageSize) {
            _dishesSelection.RemoveAt(_pageSize);
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

    private Task DeleteDish(Dish dish) {
        CookbookController.DeleteMeal(dish.Id);
        _dishesSelection.Remove(dish);
        _count--;
        return _pager?.GoToPage(_pager.CurrentPage, true) ?? Task.CompletedTask;
    }

    private void DishChanged(Guid id, string name) {
        CookbookController.SetMealName(id, name);
    }

    private void PageChanged(PagerEventArgs args) {
        var skip = args.Skip;
        var take = args.Top;
        SetDishesSelection(skip, take);
    }

    private void SetDishesSelection(int skip, int take) {
        var dishes = CookbookController.GetMeals().OrderBy(dish => dish);
        _dishesSelection.Clear();
        _dishesSelection.AddRange(dishes.Skip(skip).Take(take));
    }
}