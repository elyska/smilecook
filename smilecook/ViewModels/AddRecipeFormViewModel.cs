using Android.OS;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using IntelliJ.Lang.Annotations;
using Microsoft.Maui.Storage;
using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
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
        public AddRecipeFormViewModel(MyRecipesDBService myRecipesDBService)
        {
            this.myRecipesDBService = myRecipesDBService;
            MyRecipe = new MyRecipe();
            ImgSource = ImageSource.FromFile("placeholder.png");
        }
        [ObservableProperty]
        MyRecipe myRecipe;

        [ObservableProperty]
        ImageSource imgSource;

        [RelayCommand]
        void SaveRecipe()
        {
            myRecipesDBService.InsertRecipe(MyRecipe);
            MyRecipe = new MyRecipe();
            ImgSource = ImageSource.FromFile("placeholder.png"); 
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
