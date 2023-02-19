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
    public partial class ShoppingListViewModel : BaseViewModel
    {
        ShoppingListDBService shoppingListService;
        public ObservableCollection<ShoppingList> ShoppingList { get; } = new();
        public ShoppingListViewModel(ShoppingListDBService shoppingListService) 
        {
            this.shoppingListService = shoppingListService;

            ShoppingList = shoppingListService.GetShoppingListItems();
        }
        [ObservableProperty]
        bool isRefreshing;
        [RelayCommand]
        void GetItems()
        {
            IsRefreshing = true;
            Debug.WriteLine("Get items called");
            ShoppingList.Clear();
            var items = shoppingListService.GetShoppingListItems();
            foreach ( var item in items )
            {
                ShoppingList.Add( item );
            }
            Debug.WriteLine(ShoppingList.Count());
            IsRefreshing = false;
        }

    }
}
