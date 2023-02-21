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
    public partial class FavouritesViewModel : BaseViewModel
    {
        FavouritesDBService favouritesDBService;
        public ObservableCollection<Favourite> Favourites { get; } = new();

        public FavouritesViewModel (FavouritesDBService favouritesDBService)
        {
            this.favouritesDBService = favouritesDBService;

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
    }
}
