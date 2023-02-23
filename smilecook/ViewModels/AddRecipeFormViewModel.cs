using Android.OS;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage;
using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Java.Util.Jar.Attributes;
using static System.Net.Mime.MediaTypeNames;

namespace smilecook.ViewModels
{
    public partial class AddRecipeFormViewModel : BaseViewModel
    {
        MyRecipesDBService myRecipesDBService;
        MyIngredientDBService myIngredientDBService;
        public ObservableCollection<MyIngredient> Ingredients { get; } = new();
        public AddRecipeFormViewModel(MyRecipesDBService myRecipesDBService, MyIngredientDBService myIngredientDBService)
        {
            this.myRecipesDBService = myRecipesDBService;
            this.myIngredientDBService = myIngredientDBService; 

            MyRecipe = new MyRecipe();
            ImgSource = ImageSource.FromFile("placeholder.png");
        }
        [ObservableProperty]
        MyRecipe myRecipe;

        [ObservableProperty]
        string ingredient;

        [ObservableProperty]
        ImageSource imgSource;

        [RelayCommand]
        void SaveRecipe()
        {
            int recipeId = myRecipesDBService.InsertRecipe(MyRecipe);

            List<MyIngredient> ingredientList = Ingredients.Cast<MyIngredient>().ToList();

            foreach(var item in ingredientList)
            {
                item.MyRecipeId = recipeId;
            }
            myIngredientDBService.InsertIngredients(ingredientList);

            // reset form
            MyRecipe = new MyRecipe();
            Ingredients.Clear();
            ImgSource = ImageSource.FromFile("placeholder.png");
        }
        [RelayCommand]
        void AddIngredient()
        {
            if (Ingredient is null || Ingredient == "") return;
            System.Diagnostics.Debug.WriteLine("Add Ingredient called");
            MyIngredient newIngredient = new MyIngredient() { IngredientLine = Ingredient };
            Ingredients.Add(newIngredient);
            System.Diagnostics.Debug.WriteLine("newIngredient.MyRecipeId");
            System.Diagnostics.Debug.WriteLine(newIngredient.MyRecipeId);
            Ingredient = "";
        }

        [RelayCommand]
        async Task<FileResult> UploadPhoto(PickOptions options)
        {
            // code based on https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/storage/file-picker?view=net-maui-7.0&tabs=android
            try
            {
                var result = await FilePicker.Default.PickAsync(options);
                if (result != null)
                {
                    var stream = await result.OpenReadAsync();

                    // stream to byte string
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    byte[] bytes = ms.ToArray();
                    string byteStr = Convert.ToBase64String(bytes);
                    
                    // update image preview
                    ImgSource =  ImageSource.FromStream(() => new MemoryStream(bytes));
                    // update recipe image
                    MyRecipe.Image = byteStr;
                }

                return result;
            }
            catch (Exception ex)
            {
                // The user canceled or something went wrong
                System.Diagnostics.Debug.WriteLine($"Failed to upload. Error: {ex.Message}");
            }

            return null;
        }

        [RelayCommand]
        async Task TakePhoto()
        {
            // code based on https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device-media/picker?view=net-maui-7.0&tabs=android
            if (MediaPicker.Default.IsCaptureSupported)
            {
                IsBusy = true;
                FileResult photo = await MediaPicker.Default.CapturePhotoAsync();

                if (photo != null)
                {
                    var stream = await photo.OpenReadAsync();

                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    byte[] bytes = ms.ToArray();
                    string byteStr = Convert.ToBase64String(bytes);

                    // update recipe image
                    MyRecipe.Image = byteStr;

                    // update image preview
                    ImgSource = ImageSource.FromStream(() => new MemoryStream(bytes));
                }
            }
        }
    }
}
