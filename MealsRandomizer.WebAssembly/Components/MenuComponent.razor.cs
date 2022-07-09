namespace MealsRandomizer.WebAssembly.Components;

using System.Reactive;
using System.Reactive.Subjects;
using System.Text.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;

public partial class MenuComponent {
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private InputFile _filePicker = null!;

    private async Task ClickImport() {
        await JsRuntime.InvokeVoidAsync("triggerClick", _filePicker.Element);
    }

    private async Task Import(InputFileChangeEventArgs args) {
        await using var stream = args.File.OpenReadStream();
        using var streamReader = new StreamReader(stream);
        var content = await streamReader.ReadToEndAsync();
        var cookbook = JsonSerializer.Deserialize<Cookbook>(content)!;
        CookbookController.Cookbook = cookbook;
    }

    private async Task Export() {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(CookbookController.Cookbook);
        using var memoryStream = new MemoryStream(bytes);
        using var streamRef = new DotNetStreamReference(stream: memoryStream);
        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", "cookbook.json", streamRef);
    }
}