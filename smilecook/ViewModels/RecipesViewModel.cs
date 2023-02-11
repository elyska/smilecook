using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using smilecook.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.ViewModels
{
    public partial class RecipesViewModel : BaseViewModel
    {
        IConnectivity connectivity;
        RecipeService recipeService;

        public ObservableCollection<RecipeDetails> RecipesCol { get; } = new();
        public RecipesViewModel(RecipeService recipeService, IConnectivity connectivity)
        {
            Title = "Recipes";

            this.recipeService = recipeService;
            this.connectivity = connectivity;

            // Load data when the page appears
            IsRefreshing = true;
            Task.Run(GetRecipesAsync);
        }
        [ObservableProperty]
        bool isRefreshing;

        [RelayCommand]
        async Task SearchRecipesAsync(string searchTerm)
        {
            Debug.WriteLine("searchTerm");
            Debug.WriteLine(searchTerm);

            if (IsBusy)
                return;

            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Issue", $"Check your internet and try again.", "OK");

                    return;
                }

                IsBusy = true;
                List<RecipeHits> response = await recipeService.SearchByName(searchTerm);

                if (response.Count > 0)
                {
                    RecipesCol.Clear();
                }

                foreach (var recipe in response)
                {
                    RecipesCol.Add(recipe.Recipe);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get recipes: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }

        [RelayCommand]
        async Task GetRecipesAsync()
        {
            if (IsBusy)
                return;

            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Issue", $"Check your internet and try again.", "OK");

                    return;
                }

                IsBusy = true;
                List<RecipeHits> response = await recipeService.GetRecipes();
                
                if (response.Count > 0) 
                {
                    RecipesCol.Clear();
                }
                
                foreach (var recipe in response)
                {
                    RecipesCol.Add(recipe.Recipe);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error!", $"Unable to get recipes: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
                IsRefreshing = false;
            }
        }
    }
}
