using smilecook.ViewModels;

namespace smilecook.Views;

public partial class MyRecipesPage : ContentPage
{
	MyRecipesViewModel viewmodel;
    public MyRecipesPage(MyRecipesViewModel vm)
	{
		InitializeComponent();
		BindingContext = vm;
        viewmodel = vm;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        viewmodel.GetAllCommand.Execute(null);
    }
}