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
    public class DietDBService : DBService
    {
        public DietDBService(string dbpath) : base(dbpath)
        {
            // truncate and insert new data
            DeleteAll<Diet>();
            InsertDiet("balanced");
            InsertDiet("high-fibre");
            InsertDiet("high-protein");
            InsertDiet("low-carb");
            InsertDiet("low-fat");
            InsertDiet("low-sodium");
        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<Diet>();
        }
        public void DeleteAll<Diet>()
        {
            Init();
            conn.DeleteAll<Diet>();
        }
        public void InsertDiet(string name)
        {
            int result = 0;
            try
            {
                Init();

                // basic validation to ensure a name was entered
                if (string.IsNullOrEmpty(name))
                    throw new Exception("Valid name required");

                // enter this line
                result = conn.Insert(new Diet { Name = name });

                Debug.WriteLine($"{result} record(s) added (Name: {name})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add {name}. Error: {ex.Message}");
            }
        }
        public ObservableCollection<Diet> GetAllDiets()
        {
            try
            {
                Init();
                return conn.Table<Diet>().ToObservableCollection();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve data. {ex.Message}");
            }

            return new ObservableCollection<Diet>();
        }

    }
}
