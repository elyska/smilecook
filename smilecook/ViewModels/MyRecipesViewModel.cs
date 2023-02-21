using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.ViewModels
{
    public partial class MyRecipesViewModel : BaseViewModel
    {
        MyRecipesDBService myRecipesDBService;
        public ObservableCollection<MyRecipeImageSource> MyRecipes { get; set; } = new();
        public MyRecipesViewModel(MyRecipesDBService myRecipesDBService) 
        {
            this.myRecipesDBService = myRecipesDBService;
            MyRecipes = myRecipesDBService.GetAllMyRecipes();

        }

    }
}
