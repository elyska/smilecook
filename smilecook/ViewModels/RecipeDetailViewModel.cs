using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace smilecook.ViewModels
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class RecipeDetailViewModel : BaseViewModel
    {
        public RecipeDetailViewModel() 
        { 

        }

        [ObservableProperty]
        RecipeDetails recipe;

        [RelayCommand]
        async Task GoToLinkAsync(string link)
        {
            await Launcher.OpenAsync(link);
        }
    }
}
