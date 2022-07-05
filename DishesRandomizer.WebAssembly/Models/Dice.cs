namespace DishesRandomizer.WebAssembly.Models;

public class Dice {
    public int Rotation { get; private set; }

    public void RotateRandomly() {
        var random = new Random();
        Rotation = random.Next(0, 361);
    }

    public void Reset() {
        Rotation = 0;
    }
}