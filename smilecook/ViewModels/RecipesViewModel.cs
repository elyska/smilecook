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
        public ObservableCollection<MealType> MealTypes { get; } = new();
        public ObservableCollection<Diet> Diets { get; } = new();
        public ObservableCollection<Health> HealthLabels { get; } = new();
        public RecipesViewModel(RecipeService recipeService, IConnectivity connectivity)
        {
            //Title = "Recipes";

            this.recipeService = recipeService;
            this.connectivity = connectivity;

            //MealType.Add(new MealTypes() { Name = "Lunch" });
            //MealType.Add(new MealTypes() { Name = "Dinner" });

            // Load data when the page appears
            IsRefreshing = true;

            // add filter options
            GetMealTypes();
            GetDiets();
            GetHealthLabels();

            Task.Run(GetRecipesAsync);

        }
        [ObservableProperty]
        bool isRefreshing;

        [ObservableProperty]
        string searchTerm;

        [ObservableProperty]
        string isSelected;

        [ObservableProperty]
        bool filtersVisibility;

        [ObservableProperty]
        bool breakfastChecked;

        private void GetMealTypes()
        {
            MealTypes.Add(new MealType() { Name = "Breakfast" });
            MealTypes.Add(new MealType() { Name = "Lunch" });
            MealTypes.Add(new MealType() { Name = "Dinner" });
        }
        private void GetDiets()
        {
            Diets.Add(new Diet() { Name = "balanced" });
            Diets.Add(new Diet() { Name = "high-protein" });
            Diets.Add(new Diet() { Name = "low-sodium" });
        }
        private void GetHealthLabels()
        {
            HealthLabels.Add(new Health() { Name = "vegan" });
            HealthLabels.Add(new Health() { Name = "vegetarian" });
            HealthLabels.Add(new Health() { Name = "wheat-free" });
        }

        [RelayCommand]
        async Task SearchRecipesAsync()
        {
            Debug.WriteLine("SearchTerm");
            Debug.WriteLine(SearchTerm);

            // get filters
            List<Dictionary<string, string>> filters = new();
            // meal type filters
            var selectedMealTypes = MealTypes.Where(x => x.IsSelected).Select(x => x.Name);
            foreach (var selected in selectedMealTypes)
            {
                Debug.WriteLine($"{selected}");

                filters.Add(new Dictionary<string, string> { { "mealType", selected } });
            }
            // diet filters
            var selectedDiets = Diets.Where(x => x.IsSelected).Select(x => x.Name);
            foreach (var selected in selectedDiets)
            {
                Debug.WriteLine($"{selected}");

                filters.Add(new Dictionary<string, string> { { "diet", selected } });
            }
            // health filters
            var selectedHealth = HealthLabels.Where(x => x.IsSelected).Select(x => x.Name);
            foreach (var selected in selectedHealth)
            {
                Debug.WriteLine($"{selected}");

                filters.Add(new Dictionary<string, string> { { "health", selected } });
            }

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
                List<RecipeHits> response = new List<RecipeHits>();
                if (SearchTerm != "" && SearchTerm is not null)
                {
                    response = await recipeService.SearchByName(SearchTerm, filters);
                }
                else
                {
                    response = await recipeService.GetRecipes();
                }
                

                //if (response.Count > 0)
                //{
                    Recipes.Clear();
                //}
                Debug.WriteLine("Recipes.Count");
                Debug.WriteLine(Recipes.Count);

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
        async Task ChangeFiltersVisibility()
        {
            FiltersVisibility = !FiltersVisibility;
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
