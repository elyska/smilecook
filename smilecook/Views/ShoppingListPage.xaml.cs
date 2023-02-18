using smilecook.ViewModels;

namespace smilecook.Views;

public partial class ShoppingListPage : ContentPage
{
	public ShoppingListPage(ShoppingListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
	}
}