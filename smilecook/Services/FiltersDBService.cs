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
    public class FiltersDBService : DBService
    {
        public FiltersDBService(string dbpath) : base(dbpath)
        {
            // truncate and insert new data
            DeleteAll<Filter>();
            InsertFilter("Breakfast", "mealType");
            InsertFilter("Lunch", "mealType");
            InsertFilter("Dinner", "mealType");

            InsertFilter("balanced", "diet");
            InsertFilter("high-fibre", "diet");
            InsertFilter("high-protein", "diet");
            InsertFilter("low-carb", "diet");
            InsertFilter("low-fat", "diet");
            InsertFilter("low-sodium", "diet");

            InsertFilter("vegan", "health");
            InsertFilter("vegetarian", "health");
            InsertFilter("pescatarian", "health");
            InsertFilter("gluten-free", "health");
            InsertFilter("dairy-free", "health");
            InsertFilter("fish-free", "health");
            InsertFilter("tree-nut-free", "health");
            InsertFilter("soy-free", "health");
            InsertFilter("celery-free", "health");
            InsertFilter("red-meat-free", "health");
            /*
            InsertFilter("immuno-supportive", "health");
            InsertFilter("crustacean-free", "health");
            InsertFilter("fodmap-free", "health");
            InsertFilter("alcohol-cocktail", "health");
            InsertFilter("pork-free", "health");
            InsertFilter("wheat-free", "health");
            InsertFilter("no-oil-added", "health");
            InsertFilter("alcohol-free", "health");
            InsertFilter("peanut-free", "health");
            InsertFilter("shellfish-free", "health");
            InsertFilter("egg-free", "health");
            InsertFilter("sesame-free", "health");
            InsertFilter("low-sugar", "health");
                                                    */
        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Filter>();
        }
        public void DeleteAll<Filter>()
        {
            Init();
            conn.DeleteAll<Filter>();
        }
        public void InsertFilter(string name, string type)
        {
            int result = 0;
            try
            {
                Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                // enter this line
                result = conn.Insert(new Filter { Name = name, Type = type });

                Debug.WriteLine($"{result} record(s) added (Name: {name}, Type: {type})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add {name}. Error: {ex.Message}");
            }
        }
        public ObservableCollection<Filter> GetFiltersByType(string type)
        {
            try
            {
                Init();
                return conn.Table<Filter>().Where(i => i.Type == type).ToObservableCollection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve data. {ex.Message}");
            }

            return new ObservableCollection<Filter>();
        }
    }
}
