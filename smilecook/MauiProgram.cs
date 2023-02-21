using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using smilecook.Helpers;
using smilecook.Models;
using smilecook.Services;
using smilecook.ViewModels;
using smilecook.Views;

namespace smilecook;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            // Initialize the .NET MAUI Community Toolkit by adding the below line of code
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
#endif

        // viewmodels
        builder.Services.AddSingleton<RecipesViewModel>();
        builder.Services.AddTransient<RecipeDetailViewModel>();
        builder.Services.AddSingleton<ShoppingListViewModel>();
        builder.Services.AddSingleton<FavouritesViewModel>();
        builder.Services.AddSingleton<AddRecipeFormViewModel>();

        // pages
        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<RecipeDetailPage>();
        builder.Services.AddSingleton<ShoppingListPage>();
        builder.Services.AddSingleton<FavouritesPage>();
        builder.Services.AddSingleton<AddRecipeFormPage>();
        
        // services
        builder.Services.AddSingleton<RecipeAPIService>();
        string dbPath = FileAccessHelper.GetLocalFilePath("database.db3");
        builder.Services.AddSingleton<FiltersDBService>(s => ActivatorUtilities.CreateInstance<FiltersDBService>(s, dbPath));
        builder.Services.AddSingleton<ShoppingListDBService>(s => ActivatorUtilities.CreateInstance<ShoppingListDBService>(s, dbPath));
        builder.Services.AddSingleton<FavouritesDBService>(s => ActivatorUtilities.CreateInstance<FavouritesDBService>(s, dbPath));
        builder.Services.AddSingleton<MyRecipesDBService>(s => ActivatorUtilities.CreateInstance<MyRecipesDBService>(s, dbPath));

        return builder.Build();
	}
}
