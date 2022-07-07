namespace DishesRandomizer.WebAssembly.Components;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Common;
using Microsoft.AspNetCore.Components;
using Models;

public partial class RandomizerCard {
    private readonly Subject<bool> _randomizeSubject = new();

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private Dish? PlannedDish => CookbookController.GetPlannedDish(Day);

    [Parameter] public Day Day { get; set; }

    public Dice Dice { get; } = new();
    public bool Randomize { get; set; } = true;
    public IObservable<bool> RandomizeChanged => _randomizeSubject.AsObservable();

    protected override void OnInitialized() {
        Dice.OnRotationChanged.Subscribe(_ => StateHasChanged());
    }

    private void UpdatePlannedDish(Dish? dish) {
        if (dish == null) {
            CookbookController.DeletePlannedDish(Day);
        } else {
            CookbookController.SetPlannedDish(Day, dish);
        }
    }

    private void Shuffle() {
        CookbookController.ShufflePlannedDishes(Day);
        Dice.Shuffle();
    }
}