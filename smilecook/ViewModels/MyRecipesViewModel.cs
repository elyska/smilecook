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
        MyIngredientDBService myIngredientDBService;
        public ObservableCollection<MyRecipeImageSource> MyRecipes { get; set; } = new();
        public MyRecipesViewModel(MyRecipesDBService myRecipesDBService, MyIngredientDBService myIngredientDBService) 
        {
            this.myRecipesDBService = myRecipesDBService;
            this.myIngredientDBService = myIngredientDBService;
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
        void DeleteRecipe(int id)
        {
            Debug.WriteLine("Delete recipe called");

            // delete recipes
            int rowsAffected = myRecipesDBService.DeleteItem(id);

            if (rowsAffected > 0)
            {
                foreach (var item in MyRecipes)
                {
                    if (item.Id == id)
                    {
                        MyRecipes.Remove(item);
                        break;
                    }
                }
            }

            Debug.WriteLine(MyRecipes.Count);

            // delete ingredients
            myIngredientDBService.DeleteByRecipeId(id);

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
