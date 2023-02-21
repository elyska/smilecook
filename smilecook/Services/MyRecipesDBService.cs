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
        MyIngredientDBService myIngredientDBService;
        public MyRecipesDBService(string dbpath, MyIngredientDBService myIngredientDBService) : base(dbpath)
        {
            this.myIngredientDBService = myIngredientDBService;

        }
        private void Init()
        {
            if (conn != null)
                return;

            conn = new SQLiteConnection(_dbPath);
            conn.CreateTable<MyRecipe>();
        }
        public void DeleteAll()
        {
            Init();
            conn.DeleteAll<MyRecipe>();
        }
        public int InsertRecipe(MyRecipe myRecipe)
        {
            try
            {
                Init();

                if (myRecipe is null)
                    throw new Exception("Valid myRecipe required");

                var result = conn.Insert(new MyRecipe { Label = myRecipe.Label, Image = myRecipe.Image, Instructions = myRecipe.Instructions });
                //var result = conn.Query<MyRecipe>("INSERT INTO myRecipes(Label, Image, Instructions) VALUES (?, ?, ?) ", myRecipe.Label, myRecipe.Image, myRecipe.Instructions);
                var newlyAdded = conn.Table<MyRecipe>().Where(i => i.Label == myRecipe.Label);

                Debug.WriteLine($"Id {newlyAdded.FirstOrDefault().Id}");
                Debug.WriteLine($"{result} record(s) added (Label: {myRecipe.Label}, Image: {myRecipe.Image}, Id: )");
                return newlyAdded.FirstOrDefault().Id;

            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Failed to add. Error: {ex.Message}");
            }
            return 0;
        }

        public ObservableCollection<MyRecipeImageSource> GetAllMyRecipes()
        {
            Debug.WriteLine("GetAllMyRecipes called");
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
                        Id = recipe.Id,
                        Label = recipe.Label,
                        Instructions = recipe.Instructions,
                        Ingredients = myIngredientDBService.GetIngredientsByRecipeId(recipe.Id)
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
