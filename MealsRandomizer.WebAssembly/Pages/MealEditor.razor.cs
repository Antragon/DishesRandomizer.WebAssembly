namespace MealsRandomizer.WebAssembly.Pages;

using Controllers;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

public partial class MealEditor {
    private readonly List<string> _ingredients = new();

    private RadzenAutoComplete _lastAutocomplete = null!;
    private bool _focusLastAutocomplete;

    [Inject] private DialogService DialogService { get; set; } = null!;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    [Parameter] public Meal Meal { get; set; } = null!;
    [Parameter] public bool IsNew { get; set; }

    protected override void OnInitialized() {
        var ingredients = CookbookController.Cookbook.Meals.Values
            .SelectMany(m => m.Ingredients)
            .Select(i => i.Name)
            .Distinct();
        _ingredients.AddRange(ingredients);
        _ingredients.Sort();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender) {
        if (_focusLastAutocomplete) {
            _focusLastAutocomplete = false;
            await _lastAutocomplete.FocusAsync();
        }
    }

    private void AddIngredient() {
        Meal.Ingredients.Add(new Ingredient());
        _focusLastAutocomplete = true;
    }

    private void Save() {
        CookbookController.PutMeal(Meal);
        DialogService.Close();
    }

    private void SaveAsNew() {
        Meal.Id = Guid.NewGuid();
        CookbookController.PutMeal(Meal);
        DialogService.Close();
    }
}