using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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


    }
}
