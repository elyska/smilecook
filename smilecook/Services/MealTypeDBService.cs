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
    public class MealTypeDBService
    {
        string _dbPath;

        private SQLiteConnection conn;
        public MealTypeDBService(string dbPath) 
        {
            _dbPath = dbPath;

            // truncate and insert new data
            DeleteAllItems<MealType>();
            InsertMealType("Breakfast");
            InsertMealType("Lunch");
            InsertMealType("Dinner");
        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<MealType>();
        }
        public ObservableCollection<MealType> GetAllMealTypes()
        {
            try
            {
                Init();
                return conn.Table<MealType>().ToObservableCollection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve data. {ex.Message}" );
            }

            return new ObservableCollection<MealType>();
        }
        public void DeleteAllItems<T>()
        {
            Init();
            conn.DeleteAll<MealType>();
        }
        public void InsertMealType(string name)
        {
            int result = 0;
            try
            {
                Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                // enter this line
                result = conn.Insert(new MealType { Name = name });

                Debug.WriteLine($"{result} record(s) added (Name: {name})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add {name}. Error: {ex.Message}");
            }
        }

    }
}
