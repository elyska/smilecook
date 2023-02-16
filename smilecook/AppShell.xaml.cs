using smilecook.Views;

namespace smilecook;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

        Routing.RegisterRoute(nameof(RecipeDetailPage), typeof(RecipeDetailPage));
        Routing.RegisterRoute(nameof(FiltersPage), typeof(FiltersPage));
        Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
    }
}
