namespace DishesRandomizer.Common;

public record Dish(Guid Id, string Name) {
    public override string ToString() {
        return Name;
    }
}