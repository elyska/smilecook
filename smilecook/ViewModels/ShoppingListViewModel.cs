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

            DeleteButtonVisible = ShoppingList.Count > 0;
        }
        [ObservableProperty]
        bool deleteButtonVisible;

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
            if (ShoppingList.Count > 0)
            {
                DeleteButtonVisible = true;
            }
            Debug.WriteLine(ShoppingList.Count);
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
                if (ShoppingList.Count == 0)
                {
                    DeleteButtonVisible = false;
                }
            }

            Debug.WriteLine(ShoppingList.Count);
        }

        [RelayCommand]
        void DeleteAll()
        {
            shoppingListService.DeleteAll();
            ShoppingList.Clear();
            DeleteButtonVisible = false;
        }

    }
}
