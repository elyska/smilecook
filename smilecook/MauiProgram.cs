﻿using Microsoft.Extensions.Logging;
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
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();

        builder.Services.AddSingleton<IConnectivity>(Connectivity.Current);
#endif

        builder.Services.AddSingleton<RecipeService>();

        builder.Services.AddSingleton<RecipesViewModel>();
        builder.Services.AddTransient<RecipeDetailViewModel>();

        builder.Services.AddSingleton<MainPage>();
        builder.Services.AddTransient<RecipeDetailPage>();

        return builder.Build();
	}
}
