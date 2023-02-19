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
        [RelayCommand]
        void GetItems()
        {
            Debug.WriteLine("Get items called");
            ShoppingList.Clear();
            var items = shoppingListService.GetShoppingListItems();
            foreach (var item in items)
            {
                ShoppingList.Add(item);
            }
            Debug.WriteLine(ShoppingList.Count());
        }
        [RelayCommand]
        void DeleteItem(int id)
        {
            Debug.WriteLine("Delete items called");

            int rowsAffected = shoppingListService.DeleteItem(id);

            if (rowsAffected > 0)
            {
                foreach (var item in ShoppingList)
                {
                    if (item.Id == id)
                    {
                        ShoppingList.Remove(item);
                        break;
                    }
                }
            }

            Debug.WriteLine(ShoppingList.Count());
        }

    }
}
