using smilecook.Models;
using smilecook.Services;

namespace smilecook;

public partial class App : Application
{
    public static MealTypeDBService MealTypeService { get; private set; }
    public App(MealTypeDBService mealTypeService)
	{
		InitializeComponent();

		MainPage = new AppShell();

		MealTypeService = mealTypeService;
	}
}
