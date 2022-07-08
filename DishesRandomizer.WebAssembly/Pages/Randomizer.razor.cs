namespace DishesRandomizer.WebAssembly.Pages;

using Components;
using Microsoft.AspNetCore.Components;
using Models;
using MoreLinq;
using Radzen;

public partial class Randomizer {
    private readonly Dice _dice = new();
    private readonly IList<RandomizerCard> _cards = new List<RandomizerCard>();

    private bool? _randomize = true;

    private bool _displayDishes;

    [Inject] private DialogService DialogService { get; set; } = null!;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private RandomizerCard CardRef {
        set {
            if (_cards.Contains(value)) return;
            _cards.Add(value);
            value.RandomizeChanged.Subscribe(_ => OnCheckboxUpdated());
        }
    }

    protected override void OnInitialized() {
        _dice.OnRotationChanged.Subscribe(_ => StateHasChanged());
    }

    private void UpdateAllCheckboxes(bool? randomizeAll) {
        if (randomizeAll is not { } value) {
            return;
        }

        _cards.ForEach(c => c.Randomize = value);
    }

    private void OnCheckboxUpdated() {
        if (_cards.All(c => c.Randomize)) {
            _randomize = true;
        } else if (_cards.All(c => !c.Randomize)) {
            _randomize = false;
        } else {
            _randomize = null;
        }

        StateHasChanged();
    }

    private void ShuffleAll() {
        var cards = _cards.Where(c => c.Randomize).ToList();
        CookbookController.ShufflePlannedDishes(cards.Select(c => c.Day).ToArray());
        _dice.Shuffle();
        cards.Select(c => c.Dice).ForEach(d => d.Shuffle());
    }

    private void OpenDishes() {
        _displayDishes = true;
        StateHasChanged();
    }

    private void CloseDishes() {
        _displayDishes = false;
        StateHasChanged();
    }

    // private Task OpenDishes() {
    //     var dialogOptions = new DialogOptions {
    //         Style = "tool-window",
    //         // Width = "400px",
    //         // Top = "50px",
    //         ShowTitle = false
    //     };
    //     return DialogService.OpenAsync<Dishes>(string.Empty, options: dialogOptions);
    // }
}