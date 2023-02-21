using smilecook.ViewModels;

namespace smilecook.Views;

public partial class MyRecipesPage : ContentPage
{
	public MyRecipesPage(MyRecipesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}