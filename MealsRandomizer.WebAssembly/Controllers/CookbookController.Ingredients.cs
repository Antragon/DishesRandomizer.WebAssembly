namespace MealsRandomizer.WebAssembly.Controllers;

using MoreLinq;

public partial class CookbookController {
    public void AddIngredient(Ingredient ingredient) {
        Cookbook.Ingredients.Add(ingredient.Id, ingredient);
        _cookbookSubject.OnNext(Cookbook);
    }

    public void SetIngredientName(Guid id, string name) {
        Cookbook.Ingredients[id].Name = name;
        _cookbookSubject.OnNext(Cookbook);
    }

    public void DeleteIngredient(Guid id) {
        Cookbook.Ingredients.Remove(id);
        Cookbook.Meals.Values.ForEach(m => m.Ingredients.Remove(id));
        _cookbookSubject.OnNext(Cookbook);
    }

    public IEnumerable<Ingredient> GetIngredients() {
        return Cookbook.Ingredients.Values;
    }

    public void AssignIngredient(Guid mealId, Guid ingredientId) {
        Cookbook.Meals[mealId].Ingredients.Add(ingredientId);
        _cookbookSubject.OnNext(Cookbook);
    }

    public void UnassignIngredient(Guid mealId, Guid ingredientId) {
        Cookbook.Meals[mealId].Ingredients.Remove(ingredientId);
        _cookbookSubject.OnNext(Cookbook);
    }
}