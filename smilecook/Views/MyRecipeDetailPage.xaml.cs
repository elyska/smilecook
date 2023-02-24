using smilecook.ViewModels;

namespace smilecook.Views;

public partial class MyRecipeDetailPage : ContentPage
{
	public MyRecipeDetailPage(MyRecipeDetailViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}