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
        RecipeAPIService recipeService;
        FiltersDBService filterService;

        public ObservableCollection<RecipeDetails> Recipes { get; } = new();
        public ObservableCollection<Filter> MealTypes { get; } = new();
        public ObservableCollection<Filter> Diets { get; } = new();
        public ObservableCollection<Filter> HealthLabels { get; } = new();
        public RecipesViewModel(RecipeAPIService recipeService, FiltersDBService filterService, IConnectivity connectivity)
        {
            //Title = "Recipes";

            this.recipeService = recipeService;
            this.filterService = filterService;
            this.connectivity = connectivity;


            // Load data when the page appears
            IsRefreshing = true;

            // add filter options
            MealTypes = filterService.GetFiltersByType("mealType");
            Diets = filterService.GetFiltersByType("diet");
            HealthLabels = filterService.GetFiltersByType("health");

            Task.Run(SearchRecipesAsync);

        }
        public bool IsFilterCounterVisible => FilterCounter != 0;
        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string searchTerm;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsFilterCounterVisible))]
        int filterCounter;

        [ObservableProperty]
        bool filtersVisibility;

        int CountFilters(List<string> mealTypes, List<string> diets, List<string> healthLabels)
        {
            return mealTypes.Count() + diets.Count() + healthLabels.Count();
        }

        [RelayCommand]
        async Task SearchRecipesAsync()
        {
            IsRefreshing = true;
            if (IsBusy)
                return;

            Debug.WriteLine("SearchTerm");
            Debug.WriteLine(SearchTerm);

            // get filters
            // meal type filters
            var selectedMealTypes = MealTypes.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            // diet filters
            var selectedDiets = Diets.Where(x => x.IsSelected).Select(x => x.Name).ToList();
            // health filters
            var selectedHealth = HealthLabels.Where(x => x.IsSelected).Select(x => x.Name).ToList();

            FilterCounter = CountFilters(selectedMealTypes, selectedDiets, selectedHealth);


            try
            {
                if (connectivity.NetworkAccess != NetworkAccess.Internet)
                {
                    await Shell.Current.DisplayAlert("Internet Issue", $"Check your internet and try again.", "OK");

                    return;
                }

                IsBusy = true;
                List<RecipeHits> response = await recipeService.SearchRecipes(SearchTerm, selectedMealTypes, selectedDiets, selectedHealth);
                
                Recipes.Clear();
                
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
        void ChangeFiltersVisibility()
        {
            FiltersVisibility = !FiltersVisibility;
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
    }
}
