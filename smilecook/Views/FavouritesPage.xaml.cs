using AndroidX.Lifecycle;
using smilecook.ViewModels;

namespace smilecook.Views;

public partial class FavouritesPage : ContentPage
{
    FavouritesViewModel viewmodel;
    public FavouritesPage(FavouritesViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
        viewmodel = vm;

    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        viewmodel.GetFavouritesCommand.Execute(null);
    }
}