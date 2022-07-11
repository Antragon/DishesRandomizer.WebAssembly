namespace MealsRandomizer.WebAssembly.Components;

using System.Text.Json;
using Controllers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Radzen;

public partial class MenuComponent {
    [Inject] private IJSRuntime JsRuntime { get; set; } = null!;
    [Inject] private DialogService DialogService { get; set; } = null!;

    [CascadingParameter] private CookbookController CookbookController { get; set; } = null!;

    private InputFile _filePicker = null!;

    private async Task ClickImport() {
        await JsRuntime.InvokeVoidAsync("triggerClick", _filePicker.Element);
    }

    private async Task Import(InputFileChangeEventArgs args) {
        try {
            await using var stream = args.File.OpenReadStream();
            using var streamReader = new StreamReader(stream);
            var content = await streamReader.ReadToEndAsync();
            var cookbook = JsonSerializer.Deserialize<Cookbook>(content)!;
            CookbookController.Cookbook = cookbook;
            await ShowImportSuccessful();
        } catch (Exception) {
            await ShowImportFailed();
        }
    }

    private async Task ShowImportSuccessful() {
        var confirmOptions = new ConfirmOptions {
            OkButtonText = "Yay!",
            CancelButtonText = "Awesome!"
        };
        await DialogService.Confirm(null, "Import successful!", confirmOptions);
    }

    private async Task ShowImportFailed() {
        var confirmOptions = new ConfirmOptions {
            OkButtonText = "Darn!",
            CancelButtonText = "Oh No!"
        };
        await DialogService.Confirm("Could not read file", "Import failed...", confirmOptions);
    }

    private async Task Export() {
        var bytes = JsonSerializer.SerializeToUtf8Bytes(CookbookController.Cookbook);
        using var memoryStream = new MemoryStream(bytes);
        using var streamRef = new DotNetStreamReference(stream: memoryStream);
        await JsRuntime.InvokeVoidAsync("downloadFileFromStream", "MyCookbook.cookbook", streamRef);
    }
}