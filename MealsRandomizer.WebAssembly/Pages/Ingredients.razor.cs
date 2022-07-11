namespace MealsRandomizer.WebAssembly.Pages;

using Controllers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class Ingredients {
    private const int _pageSize = 7;
    
    private readonly List<Ingredient> _ingredientsSelection = new();
    private readonly List<RadzenTextBox> _textBoxes = new();

    private RadzenPager? _pager;
    private int _count;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private RadzenTextBox TextBoxRef {
        set => _textBoxes.Add(value);
    }

    protected override void OnInitialized() {
        SetIngredientsSelection(0, _pageSize);
        _count = CookbookController.GetIngredients().Count();
    }

    private async Task AddIngredient() {
        var ingredient = new Ingredient(Guid.NewGuid(), string.Empty);
        CookbookController.AddIngredient(ingredient);
        _ingredientsSelection.Insert(0, ingredient);
        if (_ingredientsSelection.Count > _pageSize) {
            _ingredientsSelection.RemoveAt(_pageSize);
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

    private Task DeleteIngredient(Ingredient ingredient) {
        CookbookController.DeleteIngredient(ingredient.Id);
        _ingredientsSelection.Remove(ingredient);
        _count--;
        return _pager?.GoToPage(_pager.CurrentPage, true) ?? Task.CompletedTask;
    }

    private void IngredientChanged(Guid id, string name) {
        CookbookController.SetIngredientName(id, name);
    }

    private void PageChanged(PagerEventArgs args) {
        var skip = args.Skip;
        var take = args.Top;
        SetIngredientsSelection(skip, take);
    }

    private void SetIngredientsSelection(int skip, int take) {
        var ingredients = CookbookController.GetIngredients().OrderBy(ingredient => ingredient);
        _ingredientsSelection.Clear();
        _ingredientsSelection.AddRange(ingredients.Skip(skip).Take(take));
    }
}