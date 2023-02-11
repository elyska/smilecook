using CommunityToolkit.Mvvm.ComponentModel;
using smilecook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
