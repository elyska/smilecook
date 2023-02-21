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
    public class MyIngredientDBService : DBService
    {
        public MyIngredientDBService(string dbpath) : base(dbpath)
        {
        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<MyIngredient>();
        }
        public void DeleteAll()
        {
            Init();
            conn.DeleteAll<MyIngredient>();
        }

        public void InsertIngredients(List<MyIngredient> myIngredients)
        {
            Debug.WriteLine("Insert Ingredients called");
            int result = 0;
            try
            {
                Init();

                if (myIngredients is null)
                    throw new Exception("Valid myIngredients required");

                result = conn.InsertAll(myIngredients);

                Debug.WriteLine($"{result} record(s) added");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add. Error: {ex.Message}");
            }
        }
        public ObservableCollection<MyIngredient> GetIngredientsByRecipeId(int id)
        {
            Debug.WriteLine("GetIngredientsByRecipeId called");
            
            try
            {
                Init();
                var result = conn.Table<MyIngredient>().Where(i => i.MyRecipeId == id).ToObservableCollection();
                Debug.WriteLine("MyIngredient result.Count()");
                Debug.WriteLine(result.Count());
                return result;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to find recipeId {id}. Error: {ex.Message}");
            }
            return new ObservableCollection<MyIngredient>();
        }

    }
}
