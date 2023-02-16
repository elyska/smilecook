using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using smilecook.Models;
using smilecook.Services;
using smilecook.Views;
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

        public ObservableCollection<RecipeDetails> Recipes { get; } = new();
        public RecipesViewModel(RecipeService recipeService, IConnectivity connectivity)
        {
            //Title = "Recipes";

            this.recipeService = recipeService;
            this.connectivity = connectivity;

            // Load data when the page appears
            IsRefreshing = true;
            Task.Run(GetRecipesAsync);
        }
        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string searchTerm;

        Dictionary<string, List<string>> filters2 = new Dictionary<string, List<string>>()
            {
                { "mealType", new List<string> { "breakfast" } },
                { "diet", new List<string> { "balanced", "high-protein" } },
            };

        List<Dictionary<string, string>> filters = new List<Dictionary<string, string>>
        {
            new Dictionary<string, string> { { "mealType", "Breakfast" } },
            new Dictionary<string, string> { { "diet", "balanced" } },
            new Dictionary<string, string> { { "diet", "low-sodium" } },
        };

        [RelayCommand]
        async Task SearchRecipesAsync()
        {
            Debug.WriteLine("SearchTerm");
            Debug.WriteLine(SearchTerm);

            IsRefreshing = true;
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
                List<RecipeHits> response = await recipeService.SearchByName(SearchTerm, filters);

                Debug.WriteLine("response");
                Debug.WriteLine(response);

                if (response.Count > 0)
                {
                    Recipes.Clear();
                }

                foreach (var recipe in response)
                {
                    Recipes.Add(recipe.Recipe);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                //await Shell.Current.DisplayAlert("Error!", $"Unable to get recipes: {ex.Message}", "OK");
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

                Debug.WriteLine("response");
                Debug.WriteLine(response);

                if (response.Count > 0) 
                {
                    Recipes.Clear();
                }
                
                foreach (var recipe in response)
                {
                    Recipes.Add(recipe.Recipe);
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
        async Task GoToRecipeDetailAsync(RecipeDetails recipe)
        {
            if (recipe is null)
                return;

            await Shell.Current.GoToAsync($"{nameof(RecipeDetailPage)}", true,
                new Dictionary<string, object>
                {
                    {"Recipe", recipe}
                });
        }
        [RelayCommand]
        async Task GoToFiltersAsync()
        {

            await Shell.Current.GoToAsync($"{nameof(FiltersPage)}");
        }
    }
}
