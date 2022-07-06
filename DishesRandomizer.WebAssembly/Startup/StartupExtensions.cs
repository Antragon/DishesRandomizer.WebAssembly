namespace DishesRandomizer.WebAssembly.Startup;

using Common;

public static class StartupExtensions {
    public static IServiceCollection AddPlannedDishes(this IServiceCollection services) {
        return services.AddScoped(CreatePlannedDishes);
    }
    
    private static PlannedDishes CreatePlannedDishes(IServiceProvider provider) {
        var dishes = provider.GetRequiredService<Dishes>();
        return new PlannedDishes(DefaultDishes.Create(), dishes);
    }
}