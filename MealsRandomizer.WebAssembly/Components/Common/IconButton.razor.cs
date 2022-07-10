namespace MealsRandomizer.WebAssembly.Components.Common;

using Microsoft.AspNetCore.Components;

public partial class IconButton {
    [Parameter, EditorRequired] public string ImageName { get; set; } = null!;
    [Parameter] public string? Class { get; set; }
    [Parameter] public string? Style { get; set; }
    [Parameter] public EventCallback Click { get; set; }
    [Parameter] public bool Clickable { get; set; } = true;
}