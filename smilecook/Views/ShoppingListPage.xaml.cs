using smilecook.ViewModels;

namespace smilecook.Views;

public partial class ShoppingListPage : ContentPage
{
    ShoppingListViewModel viewmodel;

    public ShoppingListPage(ShoppingListViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        viewmodel = vm;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        viewmodel.GetItemsCommand.Execute(null);
    }

}