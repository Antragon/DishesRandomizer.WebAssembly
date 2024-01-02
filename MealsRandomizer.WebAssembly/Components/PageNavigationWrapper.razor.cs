namespace MealsRandomizer.WebAssembly.Components;

using Microsoft.AspNetCore.Components;
using Radzen;

public partial class PageNavigationWrapper {
    private static readonly Page _firstPage = Enum.GetValues<Page>().Min();
    private static readonly Page _lastPage = Enum.GetValues<Page>().Max();

    private Page _currentPage = _firstPage;

    [Inject] private DialogService DialogService { get; set; } = null!;

    private void Previous() {
        _currentPage -= 1;
    }

    private void Next() {
        _currentPage += 1;
    }

    private void OpenMenu() {
        var parameters = new Dictionary<string, object>();
        var dialogOptions = new DialogOptions {
            CloseDialogOnOverlayClick = true,
            ShowTitle = false,
            Style = "min-height:auto;min-width:auto;width:auto"
        };
        DialogService.Open<MenuComponent>(nameof(Meal), parameters, dialogOptions);
    }
}