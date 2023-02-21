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
using System.Windows.Input;

namespace smilecook.ViewModels
{
    [QueryProperty("Recipe", "Recipe")]
    public partial class RecipeDetailViewModel : BaseViewModel
    {
        ShoppingListDBService shoppingListService;
        FavouritesDBService favouritesDBService;

        public RecipeDetailViewModel(ShoppingListDBService shoppingListService, FavouritesDBService favouritesDBService) 
        { 
            this.shoppingListService = shoppingListService;
            this.favouritesDBService = favouritesDBService;
        }

        [ObservableProperty]
        RecipeDetails recipe;


        [RelayCommand]
        void AddToFavourites()
        {
            Debug.WriteLine("Add to favourites command");
            favouritesDBService.InsertFavourite(Recipe.Url, Recipe.Label, Recipe.Image);
            Recipe.IsFavourite = true;
        }

        [RelayCommand]
        void AddToShoppingList(string ingredient)
        {
            Debug.WriteLine("Add to shopping list command");
            Debug.WriteLine(ingredient);
            shoppingListService.InsertItem(ingredient);
        }

        [RelayCommand]
        async Task GoToLinkAsync(string link)
        {
            await Launcher.OpenAsync(link);
        }
    }
}
