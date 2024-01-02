using Newtonsoft.Json.Linq;

var content = File.ReadAllText("C:\\Users\\Felix\\Downloads\\MyCookbook.cookbook");
var cookbook = JObject.Parse(content);
var ingredients = cookbook["Ingredients"]!.ToObject<Dictionary<Guid, JObject>>()!;

foreach (var meal in cookbook["Meals"]!.Select(m => m.First!)) {
    var mealIngredients = meal["Ingredients"]!.ToObject<Dictionary<Guid, decimal?>>()!;
    var migratedMealIngredients = mealIngredients
        .Select(i => new { Name = ingredients[i.Key]["Name"]!.ToObject<string>()!, Amount = i.Value });
    meal["Ingredients"] = JArray.FromObject(migratedMealIngredients);
}

cookbook.Remove("Ingredients");
File.WriteAllText("C:\\Users\\Felix\\Downloads\\MyCookbook_migrated.cookbook", cookbook.ToString());