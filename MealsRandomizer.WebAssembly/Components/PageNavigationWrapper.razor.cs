namespace MealsRandomizer.WebAssembly.Components;

public partial class PageNavigationWrapper {
    private static readonly Page _firstPage = Enum.GetValues<Page>().Min();
    private static readonly Page _lastPage = Enum.GetValues<Page>().Max();
    
    private bool _displayMenu;
    private Page _currentPage = _firstPage;

    private void Previous() {
        _currentPage -= 1;
    }

    private void Next() {
        _currentPage += 1;
    }
}