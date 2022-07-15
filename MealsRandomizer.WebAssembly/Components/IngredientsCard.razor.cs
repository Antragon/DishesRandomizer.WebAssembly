namespace MealsRandomizer.WebAssembly.Components;

using Controllers;
using Microsoft.AspNetCore.Components;

public partial class IngredientsCard {
    private readonly List<Assignment> _selection = new();

    private bool _expand;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private Meal? Meal => CookbookController.GetPlannedMeal(Day);

    [Parameter, EditorRequired] public Day Day { get; set; }

    protected override void OnInitialized() {
        var meal = CookbookController.GetPlannedMeal(Day);
        if (meal != null) {
            var assignments = CookbookController.GetAssignments(meal.Id);
            _selection.AddRange(assignments);
            StateHasChanged();
        }
    }

    private void Add() {
        _selection.Add(new Assignment());
    }

    private void Delete(Assignment assignment) {
        _selection.Remove(assignment);
        AssignIngredient(assignment, null);
    }

    private void AssignIngredient(Assignment assignment, Ingredient? ingredient) {
        if (ingredient != null) {
            CookbookController.AssignIngredient(Meal!.Id, ingredient.Id, assignment.Amount);
        } else if (assignment.Ingredient != null) {
            CookbookController.UnassignIngredient(Meal!.Id, assignment.Ingredient.Id);
        }

        assignment.Ingredient = ingredient;
    }

    private void AssignAmount(Assignment assignment, decimal? amount) {
        if (assignment.Ingredient != null) {
            CookbookController.AssignIngredient(Meal!.Id, assignment.Ingredient.Id, amount);
        }

        assignment.Amount = amount;
    }
}