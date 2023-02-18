using CommunityToolkit.Maui.Core.Extensions;
using smilecook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Services
{
    public class ShoppingListDBService : DBService
    {
        public ShoppingListDBService(string dbpath) : base(dbpath)
        { 

        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<ShoppingList>();
        }
        public void DeleteAll<ShoppingList>()
        {
            Init();
            conn.DeleteAll<ShoppingList>();
        }
        public void InsertItem(string ingredient)
        {
            Debug.WriteLine("Insert function");
            Debug.WriteLine(ingredient);
            int result = 0;
            try
            {
                Init();

                // basic validation to ensure an ingredient was entered
                if (string.IsNullOrEmpty(ingredient))
                    throw new Exception("Valid ingredient required");

                result = conn.Insert(new ShoppingList { Ingredient = ingredient });

                Debug.WriteLine($"{result} record(s) added (Name: {ingredient})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add {ingredient}. Error: {ex.Message}");
            }
        }

        public ObservableCollection<ShoppingList> GetShoppingListItems()
        {
            try
            {
                Init();
                return conn.Table<ShoppingList>().ToObservableCollection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve data. {ex.Message}");
            }

            return new ObservableCollection<ShoppingList>();
        }
    }
}
