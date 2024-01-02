namespace MealsRandomizer.WebAssembly;

using Newtonsoft.Json;

public static class Extensions {
    public static T Clone<T>(this T original) {
        var serialized = JsonConvert.SerializeObject(original);
        var clone = JsonConvert.DeserializeObject<T>(serialized)!;
        return clone;
    }
}