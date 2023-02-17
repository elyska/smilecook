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

        builder.Services.AddSingleton<RecipeAPIService>();

        builder.Services.AddSingleton<RecipesViewModel>();
        builder.Services.AddTransient<RecipeDetailViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<RecipeDetailPage>();
        
        string dbPath = FileAccessHelper.GetLocalFilePath("database.db3");
        builder.Services.AddSingleton<MealTypeDBService>(s => ActivatorUtilities.CreateInstance<MealTypeDBService>(s, dbPath));
        builder.Services.AddSingleton<DietDBService>(s => ActivatorUtilities.CreateInstance<DietDBService>(s, dbPath));
        builder.Services.AddSingleton<FiltersDBService>(s => ActivatorUtilities.CreateInstance<FiltersDBService>(s, dbPath));

        return builder.Build();
	}
}
