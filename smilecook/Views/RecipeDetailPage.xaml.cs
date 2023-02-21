using smilecook.ViewModels;
using System.Diagnostics;

namespace smilecook.Views;

public partial class RecipeDetailPage : ContentPage
{
    public RecipeDetailPage(RecipeDetailViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
    }

}