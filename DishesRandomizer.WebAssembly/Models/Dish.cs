namespace DishesRandomizer.WebAssembly.Models;

public record Dish(Guid Id, string Name) {
    public override string ToString() {
        return Name;
    }
}