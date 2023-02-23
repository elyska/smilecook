using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.ViewModels
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class MyRecipeDetailViewModel : BaseViewModel
    {
        ShoppingListDBService shoppingListService;
        MyIngredientDBService myIngredientDBService;
        public ObservableCollection<MyIngredient> Ingredients { get; set; } = new();
        public MyRecipeDetailViewModel(ShoppingListDBService shoppingListService, MyIngredientDBService myIngredientDBService) 
        {
            this.shoppingListService = shoppingListService;
            this.myIngredientDBService = myIngredientDBService;
            
        }

        [ObservableProperty]
        MyRecipeImageSource recipe;
        partial void OnRecipeChanged(MyRecipeImageSource value)
        {
            Debug.WriteLine("value.Id");
            Debug.WriteLine(value.Id);
            Ingredients = myIngredientDBService.GetIngredientsByRecipeId(value.Id);
        }


        [RelayCommand]
        void AddToShoppingList(string ingredient)
        {
            Debug.WriteLine("Add to shopping list command");
            Debug.WriteLine(ingredient);
            shoppingListService.InsertItem(ingredient);
        }
    }
}
