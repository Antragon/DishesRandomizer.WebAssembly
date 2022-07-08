namespace MealsRandomizer.WebAssembly.Components;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Microsoft.AspNetCore.Components;
using Models;

public partial class RandomizerCard {
    private readonly Subject<bool> _randomizeSubject = new();

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private Dish? PlannedMeal => CookbookController.GetPlannedMeal(Day);

    [Parameter] public Day Day { get; set; }

    public Dice Dice { get; } = new();
    public bool Randomize { get; set; } = true;
    public IObservable<bool> RandomizeChanged => _randomizeSubject.AsObservable();

    protected override void OnInitialized() {
        Dice.OnRotationChanged.Subscribe(_ => StateHasChanged());
    }

    private void UpdatePlannedMeal(Dish? meal) {
        if (meal == null) {
            CookbookController.DeletePlannedMeal(Day);
        } else {
            CookbookController.SetPlannedMeal(Day, meal.Id);
        }
    }

    private void Shuffle() {
        CookbookController.ShufflePlannedMeals(Day);
        Dice.Shuffle();
    }
}