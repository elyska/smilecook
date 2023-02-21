using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using smilecook.Services;
using smilecook.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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
        [RelayCommand]
        void GetAll()
        {
            Debug.WriteLine("GetAll command for MyRecipes called");
            var items = myRecipesDBService.GetAllMyRecipes();
            MyRecipes.Clear();
            foreach (var item in items)
            {
                MyRecipes.Add(item);
            }
            Debug.WriteLine(MyRecipes.Count());
        }

        [RelayCommand]
        async Task GoToRecipeDetailAsync(MyRecipeImageSource recipe)
        {
            if (recipe is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(MyRecipeDetailPage)}", true,
                new Dictionary<string, object>
                {
                    {"Recipe", recipe}
                });
        }
        [RelayCommand]
        async Task GoToAddRecipe()
        {
            await Shell.Current.GoToAsync($"{nameof(AddRecipeFormPage)}", true);
        }

    }
}
