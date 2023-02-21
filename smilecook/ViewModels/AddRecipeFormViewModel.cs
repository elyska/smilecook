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
        }
        [ObservableProperty]
        string imgSource;

        [ObservableProperty]
        ImageSource imgSource2;

        [ObservableProperty]
        MyRecipe myRecipe;

        [RelayCommand]
        void SaveRecipe()
        {
            myRecipesDBService.InsertRecipe(MyRecipe);
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
                    //if (result.FileName.EndsWith("jpg", StringComparison.OrdinalIgnoreCase) ||
                        //result.FileName.EndsWith("png", StringComparison.OrdinalIgnoreCase))
                    //{
                    var stream = await result.OpenReadAsync();

                    byte[] bytes;
                    var ms = new MemoryStream();
                    stream.CopyTo(ms);
                    bytes = ms.ToArray();
                    string byteStr = Convert.ToBase64String(bytes);
                    //System.Diagnostics.Debug.WriteLine($"Image byte string: {byteStr}");
                    byte[] convertedBytes = Convert.FromBase64String(byteStr);
                    //ImgSource = byteStr;
                    
                    ImageSource image = ImageSource.FromStream(() => new MemoryStream(convertedBytes));
                        System.Diagnostics.Debug.WriteLine($"Image path: {image}");
                    //}
                    ImgSource2 = image;
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
                    // save the file into local storage
                    string localFilePath = Path.Combine(FileSystem.CacheDirectory, photo.FileName);

                    Stream sourceStream = await photo.OpenReadAsync();
                    FileStream localFileStream = File.OpenWrite(localFilePath);

                    await sourceStream.CopyToAsync(localFileStream);

                    //ImgSource = localFilePath;


                    var stream = await photo.OpenReadAsync();
                    ImageSource image = ImageSource.FromStream(() => stream);

                    ImgSource2 = image;
                }
            }
        }
    }
}
