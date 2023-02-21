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
    public partial class FavouritesViewModel : BaseViewModel
    {
        FavouritesDBService favouritesDBService;
        RecipeAPIService recipeAPIService;
        public ObservableCollection<Favourite> Favourites { get; } = new();

        public FavouritesViewModel (FavouritesDBService favouritesDBService, RecipeAPIService recipeAPIService)
        {
            this.favouritesDBService = favouritesDBService;
            this.recipeAPIService = recipeAPIService;

            Favourites = favouritesDBService.GetAllFavourites();
        }
        [RelayCommand]
        void GetFavourites()
        {
            Debug.WriteLine("Get Favourites called");
            Favourites.Clear();
            var items = favouritesDBService.GetAllFavourites();
            foreach (var item in items)
            {
                Favourites.Add(item);
            }
            Debug.WriteLine(Favourites.Count);
        }
        [RelayCommand]
        async Task GoToRecipeDetailAsync(Favourite favourite)
        {
            if (favourite is null)
                return;

            // get Recipe Details
            RecipeDetails recipe = await recipeAPIService.SearchFavouriteRecipe(favourite.Label, favourite.Url);
            
            // navigate to page
            await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}", true,
                new Dictionary<string, object>
                {
                    {"Recipe", recipe}
                });
        }
    }
}
