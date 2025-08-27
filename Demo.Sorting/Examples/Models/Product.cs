using Helpers.Sorting.Abstractions;
using Helpers.Sorting.Abstractions.Json;

namespace Demo.Sorting.Examples.Models;

public class Product : IJsonComparableSortableEntity<int>, ISortableEntity<int>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int CategoryId { get; set; }

    public DateTime? LastModified { get; set; }

    public Product() { }
    public Product(int id, string name, decimal price, DateTime? lastModified = null)
    {
        Id = id;
        Name = name;
        Price = price;
        LastModified = lastModified ?? DateTime.UtcNow;
    }

    public static List<Product> CreateDemoProducts(int amount = 100)
    {
        var products = new List<Product>();
        var random = new Random();
        var categories = new[] { 1, 2, 3, 4, 5 };

        for (int i = 1; i <= amount; i++)
        {
            var id = i;
            var name = $"Product {i}";
            var price = Math.Round((decimal)(random.NextDouble() * 100), 2);
            var lastModified = DateTime.UtcNow.AddMinutes(-random.Next(0, 1000));
            var product = new Product(id, name, price, lastModified)
            {
                CategoryId = categories[random.Next(categories.Length)]
            };

            products.Add(product);
        }

        return products;
    }

    public static List<Product> CreateModifiedProducts(List<Product> baseProducts, ProductModificationOptions? modifications = null)
    {
        if (baseProducts == null || !baseProducts.Any())
            return new List<Product>();

        var options = modifications ?? new ProductModificationOptions();
        var random = new Random();
        var modifiedProducts = new List<Product>();

        // 1. KOPIERE ALLE NICHT-ZU-LÖSCHENDEN PRODUKTE
        var productsToKeep = baseProducts.ToList();

        // Bestimme welche Produkte gelöscht werden (DELETE Simulation)
        var deleteCount = Math.Min(options.DeleteCount, baseProducts.Count / 3); // Max 1/3 löschen
        var productsToDelete = baseProducts
            .OrderBy(_ => random.Next())
            .Take(deleteCount)
            .Select(p => p.Id)
            .ToHashSet();

        // Entferne zu löschende Produkte aus der Ergebnisliste
        productsToKeep = productsToKeep.Where(p => !productsToDelete.Contains(p.Id)).ToList();

        // 2. MODIFIZIERE BESTEHENDE PRODUKTE (UPDATE Simulation)
        var updateCount = Math.Min(options.UpdateCount, productsToKeep.Count);
        var productsToUpdate = productsToKeep
            .OrderBy(_ => random.Next())
            .Take(updateCount)
            .ToList();

        foreach (var product in productsToKeep)
        {
            var newProduct = new Product
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                CategoryId = product.CategoryId,
                LastModified = product.LastModified
            };

            // Wenn dieses Produkt aktualisiert werden soll
            if (productsToUpdate.Any(p => p.Id == product.Id))
            {
                // Verschiedene Update-Szenarien
                var updateType = random.Next(1, 5);

                switch (updateType)
                {
                    case 1: // Preisänderung
                        var priceChange = (decimal)(random.NextDouble() * 40 - 20); // ±20
                        newProduct.Price = Math.Max(0.01m, product.Price + priceChange);
                        newProduct.LastModified = DateTime.UtcNow.AddMinutes(-random.Next(0, 60));
                        break;

                    case 2: // Namensänderung
                        newProduct.Name = $"{product.Name} (Updated)";
                        newProduct.LastModified = DateTime.UtcNow.AddMinutes(-random.Next(0, 30));
                        break;

                    case 3: // Kategorie-Wechsel
                        newProduct.CategoryId = random.Next(1, 6);
                        newProduct.LastModified = DateTime.UtcNow.AddMinutes(-random.Next(0, 45));
                        break;

                    case 4: // Kombinierte Änderung
                        newProduct.Name = $"Enhanced {product.Name}";
                        newProduct.Price = Math.Round(product.Price * (decimal)(0.9 + random.NextDouble() * 0.4), 2); // ±20%
                        newProduct.CategoryId = random.Next(1, 6);
                        newProduct.LastModified = DateTime.UtcNow.AddMinutes(-random.Next(0, 15));
                        break;
                }

                // Manchmal auch ältere LastModified setzen (für LastModified-Tests)
                if (options.IncludeOlderUpdates && random.NextDouble() < 0.3)
                {
                    newProduct.LastModified = product.LastModified?.AddHours(-random.Next(1, 48));
                }
            }

            modifiedProducts.Add(newProduct);
        }

        // 3. FÜGE NEUE PRODUKTE HINZU (INSERT Simulation)
        var maxId = baseProducts.Max(p => p.Id);
        for (int i = 1; i <= options.InsertCount; i++)
        {
            var newId = maxId + i;
            modifiedProducts.Add(new Product(
                id: newId,
                name: $"New Product {newId}",
                price: Math.Round((decimal)(random.NextDouble() * 150), 2), // Neue Produkte etwas teurer
                lastModified: DateTime.UtcNow.AddMinutes(-random.Next(0, 120))
            )
            {
                CategoryId = random.Next(1, 6)
            });
        }

        // 4. OPTIONAL: SHUFFLE DIE REIHENFOLGE
        if (options.ShuffleOrder)
        {
            modifiedProducts = modifiedProducts.OrderBy(_ => random.Next()).ToList();
        }

        return modifiedProducts;
    }

    public static List<Product> CreateTestScenario(List<Product> baseProducts, TestScenarioType scenarioType)
    {
        return scenarioType switch
        {
            TestScenarioType.OnlyUpdates => CreateModifiedProducts(baseProducts, new ProductModificationOptions
            {
                UpdateCount = Math.Min(10, baseProducts.Count / 2),
                InsertCount = 0,
                DeleteCount = 0
            }),

            TestScenarioType.OnlyInserts => CreateModifiedProducts(baseProducts, new ProductModificationOptions
            {
                UpdateCount = 0,
                InsertCount = 15,
                DeleteCount = 0
            }),

            TestScenarioType.OnlyDeletes => CreateModifiedProducts(baseProducts, new ProductModificationOptions
            {
                UpdateCount = 0,
                InsertCount = 0,
                DeleteCount = Math.Min(5, baseProducts.Count / 4)
            }),

            TestScenarioType.MixedOperations => CreateModifiedProducts(baseProducts, new ProductModificationOptions
            {
                UpdateCount = 8,
                InsertCount = 5,
                DeleteCount = 3,
                IncludeOlderUpdates = true
            }),

            TestScenarioType.LargeChanges => CreateModifiedProducts(baseProducts, new ProductModificationOptions
            {
                UpdateCount = baseProducts.Count / 3,
                InsertCount = 25,
                DeleteCount = baseProducts.Count / 4,
                ShuffleOrder = true
            }),

            TestScenarioType.NoChanges => new List<Product>(baseProducts), // Exakte Kopie

            _ => CreateModifiedProducts(baseProducts)
        };
    }

    public override string ToString()
    {
        return $"Product {Id}: {Name} - ${Price:F2} (Cat: {CategoryId}, Modified: {LastModified:yyyy-MM-dd HH:mm})";
    }

    public override bool Equals(object? obj)
    {
        if (obj is Product other)
        {
            return Id == other.Id;
        }

        return false;
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
}
