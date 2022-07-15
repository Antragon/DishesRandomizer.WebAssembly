namespace MealsRandomizer.WebAssembly.Controllers;

using System.Reactive.Linq;
using System.Reactive.Subjects;

public partial class CookbookController {
    private readonly Subject<Cookbook> _cookbookSubject = new();
    
    private Cookbook _cookbook;

    public CookbookController(Cookbook cookbook) {
        _cookbook = cookbook;
    }

    public Cookbook Cookbook {
        get => _cookbook;
        set => SetCookbook(value);
    }

    private void SetCookbook(Cookbook value) {
        _cookbook = value;
        _cookbookSubject.OnNext(Cookbook);
    }

    public IObservable<Cookbook> CookbookChanged => _cookbookSubject.AsObservable();
}