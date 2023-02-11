using smilecook.ViewModels;

namespace smilecook.Views;

public partial class RecipeDetailPage : ContentPage
{
	public RecipeDetailPage(RecipeDetailViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}
}