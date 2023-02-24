using Android.OS;
using smilecook.ViewModels;

namespace smilecook.Views;

public partial class AddRecipeFormPage : ContentPage
{
    public AddRecipeFormPage(AddRecipeFormViewModel vm)
	{
		InitializeComponent();
        BindingContext = vm;
	}
    
}