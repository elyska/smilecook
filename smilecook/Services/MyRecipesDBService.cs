using CommunityToolkit.Maui.Core.Extensions;
using smilecook.Models;
using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Dynamic;
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

        public ObservableCollection<MyRecipeImageSource> GetAllMyRecipes()
        {
            try
            {
                Init();
                ObservableCollection<MyRecipe> result = conn.Table<MyRecipe>().ToObservableCollection();
                // convert byte string to image source
                ObservableCollection <MyRecipeImageSource> recipes = new ObservableCollection<MyRecipeImageSource>();
                foreach (var recipe in result)
                {
                    Debug.WriteLine(recipe.Label);
                    ImageSource source;
                    if (recipe.Image != null)
                    {
                        byte[] imgBytes = Convert.FromBase64String(recipe.Image);
                        Debug.WriteLine(imgBytes);
                        source = ImageSource.FromStream(() => new MemoryStream(imgBytes));
                    }
                    else
                    {
                        source = ImageSource.FromFile("placeholder.png");
                    }

                    MyRecipeImageSource newRecipe = new MyRecipeImageSource()
                    {
                        ImgSource = source,
                        Label = recipe.Label
                    };
                    recipes.Add(newRecipe);
                }
                return recipes;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to retrieve data. {ex.Message}");
            }

            return new ObservableCollection<MyRecipeImageSource>();
        }
    }
}
