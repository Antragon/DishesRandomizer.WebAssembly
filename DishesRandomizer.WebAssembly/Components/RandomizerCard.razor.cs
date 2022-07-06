namespace DishesRandomizer.WebAssembly.Components;

using System.Reactive.Linq;
using System.Reactive.Subjects;
using Common;
using Microsoft.AspNetCore.Components;
using Models;

public partial class RandomizerCard {
    private readonly Subject<bool> _randomizeSubject = new();
    private readonly Dice _dice = new();

    [Inject] private PlannedDishes PlannedDishes { get; set; } = new();
    [Inject] private Dishes Dishes { get; set; } = new();

    [Parameter] public Day Day { get; set; }

    public bool Randomize { get; set; } = true;
    public IObservable<bool> RandomizeChanged => _randomizeSubject.AsObservable();

    protected override void OnInitialized() {
        _dice.OnRotationChanged.Subscribe(_ => StateHasChanged());
    }

    public void Shuffle() {
        _dice.Shuffle();
    }
}