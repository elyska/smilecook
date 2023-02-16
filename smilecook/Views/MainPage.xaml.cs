using smilecook.ViewModels;
using System.Diagnostics;

namespace smilecook;

public partial class MainPage : ContentPage
{
    RecipesViewModel viewModel;
	public MainPage(RecipesViewModel vm)
	{
		InitializeComponent();

		BindingContext = vm;
        viewModel = vm;

    }
    void OnCheckBoxCheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        Debug.WriteLine("e");
        Debug.WriteLine(e.Value);

        viewModel.SearchRecipesCommand.Execute(null);
    }
}

