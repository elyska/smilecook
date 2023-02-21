using smilecook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Services
{
    public class MyRecipesDBService : DBService
    {
        public MyRecipesDBService(string dbpath) : base(dbpath)
        {
        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<MyRecipe>();
        }
        public void InsertRecipe(MyRecipe myRecipe)
        {
            int result = 0;
            try
            {
                Init();

                if (myRecipe is null)
                    throw new Exception("Valid myRecipe required");

                result = conn.Insert(new MyRecipe { Label = myRecipe.Label, Image = myRecipe.Image });

                Debug.WriteLine($"{result} record(s) added (Label: {myRecipe.Label}, Image: {myRecipe.Image})");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add. Error: {ex.Message}");
            }
        }
    }
}
