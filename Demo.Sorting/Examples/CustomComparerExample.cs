using Demo.Sorting.Examples.Models;
using Helpers.Sorting.Common;
using Helpers.Sorting.Models;
using Helpers.Sorting.Services;

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

        LogHelper.WriteLog("Base Products", () =>
        {
            Console.WriteLine($"\tCreated base products: {baseProducts.Count}");
            Console.WriteLine($"\tCreated modified products: {modifiedProducts.Count}");
        });


        SortWithSettings("#1 UpdateStrategy - AlwaysUpdate", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.AlwaysUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings("#2 UpdateStrategy - NeverUpdate", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.NeverUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings("#3 UpdateStrategy - LastModifiedWins", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.LastModifiedWins,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings("#4 UpdateStrategy - CustomComparison", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.CustomComparison,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOnly,
        });

        SortWithSettings("#5 UpdateDetectionStrategy - JsonContentAndLastModified", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.AlwaysUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentAndLastModified,
        });

        SortWithSettings("#6 UpdateDetectionStrategy - LastModifiedOnly", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.NeverUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.LastModifiedOnly,
        });

        SortWithSettings("#7 UpdateDetectionStrategy - JsonContentOrLastModified", baseProducts, modifiedProducts, new JsonDatabaseSortOptions()
        {
            UpdateStrategy = UpdateStrategy.NeverUpdate,
            UpdateDetectionStrategy = UpdateDetectionStrategy.JsonContentOrLastModified,
        });

    }

    private static void SortWithSettings(string testName, List<Product> baseProducts, List<Product> modifiedProducts, JsonDatabaseSortOptions options)
    {
        var sorter = SorterServiceFactory.CreateJsonComparableDatabaseSorter<Product, int>(options);
        var sortResult = sorter.SortForDatabase(baseProducts, modifiedProducts);

        LogHelper.WriteLog(testName, () =>
        {
            Console.WriteLine($"\tSort settings: {options.UpdateStrategy}, {options.UpdateDetectionStrategy}");
            Console.WriteLine($"\tSort result: Insert/Update/Delete: {sortResult.InsertCount}/{sortResult.UpdateCount}/{sortResult.DeleteCount}");
        });
    }


}
