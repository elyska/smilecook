using smilecook.ViewModels;

namespace smilecook;

public partial class MainPage : ContentPage
{
	public MainPage(RecipesViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
	}

}

