namespace MealsRandomizer.WebAssembly.Components;

using Microsoft.AspNetCore.Components;
using Models;

public partial class DiceComponent {
    [Parameter, EditorRequired] public Dice Dice { get; set; } = null!;
    [Parameter, EditorRequired] public EventCallback Click { get; set; }
}