using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
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
        public MyRecipeDetailViewModel(ShoppingListDBService shoppingListService) 
        {
            this.shoppingListService = shoppingListService; 
        }

        [ObservableProperty]
        MyRecipeImageSource recipe;

        [RelayCommand]
        void AddToShoppingList(string ingredient)
        {
            Debug.WriteLine("Add to shopping list command");
            Debug.WriteLine(ingredient);
            shoppingListService.InsertItem(ingredient);
        }
    }
}
