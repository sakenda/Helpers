using Demo.Sorting.Examples.Models;
using Helpers.Sorting.Common;
using Helpers.Sorting.Models;
using Helpers.Sorting.Services;
using System.Drawing;
using System.Globalization;

namespace Demo.Sorting.Examples;

public static class CustomComparerExample
{
    public static void Run()
    {
        const int SIZE = 10;
        const int toUpdate = 10;
        const int toInsert = 0;
        const int toDelete = 0;

        var baseProducts = Product.CreateDemoProducts(SIZE);
        var modifiedProducts = Product.CreateModifiedProducts(
            baseProducts,
            modifications: new ProductModificationOptions
            {
                UpdateCount = toUpdate,
                InsertCount = toInsert,
                DeleteCount = toDelete
            });

        Console.WriteLine($"Created base products: {baseProducts.Count}");
        Console.WriteLine($"Created modified products: {modifiedProducts.Count}");

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.AlwaysUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.NeverUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.LastModifiedWins,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.CustomComparison,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.AlwaysUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentAndLastModified,
        });

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.NeverUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.LastModifiedOnly,
        });

        SortWithSettings(baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            LogJsonComparisons = true,
            UpdateStrategy = UpdateStrategy.NeverUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOrLastModified,
        });

    }

    private static void SortWithSettings(List<Product> baseProducts, List<Product> modifiedProducts, JsonDatabaseSortOptions options)
    {
        // ┌┐─└┘│
        var sorter = SorterServiceFactory.CreateJsonComparableDatabaseSorter<Product, int>(options);
        var sortResult = sorter.SortForDatabase(baseProducts, modifiedProducts);

        Console.WriteLine($"""
            ┌──────────────────────┘
            └┐
             │  Sort settings:
             │      {options.UpdateStrategy}, {options.UpdateDetectionStrategy}
             │  Sort result:
             │      Insert/Update/Delete: {sortResult.InsertCount}/{sortResult.UpdateCount}/{sortResult.DeleteCount}
            ┌┘
            └──────────────────────┐
            """);
    }

}
