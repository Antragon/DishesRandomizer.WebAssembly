namespace DishesRandomizer.WebAssembly.Components;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Common;
using Microsoft.AspNetCore.Components;
using Models;

public partial class RandomizerCard {
    private readonly Subject<bool> _randomizeSubject = new();

    [Inject] private PlannedDishes PlannedDishes { get; set; } = default!;
    [Inject] private Dishes Dishes { get; set; } = default!;

    private Dish? PlannedDish => PlannedDishes.TryGetValue(Day, out var dish) ? dish : null;

    [Parameter] public Day Day { get; set; }

    public Dice Dice { get; } = new();
    public bool Randomize { get; set; } = true;
    public IObservable<bool> RandomizeChanged => _randomizeSubject.AsObservable();

    protected override void OnInitialized() {
        Dice.OnRotationChanged.Subscribe(_ => StateHasChanged());
    }

    private void UpdatePlannedDish(Dish? dish) {
        if (dish == null) {
            PlannedDishes.Remove(Day);
        } else {
            PlannedDishes[Day] = dish;
        }
    }

    private void Shuffle() {
        PlannedDishes.Shuffle(Day);
        Dice.Shuffle();
    }
}