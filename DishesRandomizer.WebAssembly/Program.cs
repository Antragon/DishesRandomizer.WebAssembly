using DishesRandomizer.Common;
using DishesRandomizer.WebAssembly;
using DishesRandomizer.WebAssembly.Startup;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services
    .AddScoped(_ => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) })
    .AddScoped(_ => Dishes.Default)
    .AddPlannedDishes();

await builder.Build().RunAsync();