using Microsoft.AspNetCore.WebUtilities;
using smilecook.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Services
{

    public class RecipeAPIService
    {
        FavouritesDBService favouritesDBService;
        HttpClient httpClient;

        string url = "https://api.edamam.com/api/recipes/v2";
        Dictionary<string, string> queryParams = new Dictionary<string, string>() // default query parameters
        {
            {"app_id", "a4aac7a2" },
            {"app_key", "fc46385a390444b36f97eb6c55e833b8" },
            {"type","public" },
        };

        public RecipeAPIService(FavouritesDBService favouritesDBService)
        {
            this.favouritesDBService = favouritesDBService;

            httpClient = new HttpClient();

            url = QueryHelpers.AddQueryString(url, queryParams); // add default query parameters
        }
        public async Task<List<RecipeHits>> SearchRecipes(string searchTerm, List<string> mealTypes, List<string> diets, List<string> health) 
        {
            string requestUrl = url;

            // add search term to query params
            if (searchTerm != "" && searchTerm is not null)
            {
                var optionalQueryParams = new Dictionary<string, string>()
                {
                    {"q", searchTerm }
                };
                requestUrl = QueryHelpers.AddQueryString(requestUrl, optionalQueryParams);
            }

            // add filters to query params
            foreach (var item in mealTypes)
            {
                Debug.WriteLine($"{item}");
                requestUrl = QueryHelpers.AddQueryString(requestUrl, new Dictionary<string, string> { { "mealType", item } });

            }
            foreach (var item in diets)
            {
                Debug.WriteLine($"{item}");
                requestUrl = QueryHelpers.AddQueryString(requestUrl, new Dictionary<string, string> { { "diet", item } });
            }
            foreach (var item in health)
            {
                Debug.WriteLine($"{item}");
                requestUrl = QueryHelpers.AddQueryString(requestUrl, new Dictionary<string, string> { { "health", item } });
            }
            // if no parameters added, get "random" dinner recipes
            if ((searchTerm == "" || searchTerm is null) && mealTypes.Count == 0 && diets.Count == 0 && health.Count == 0)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, new Dictionary<string, string>() { { "mealType", "Dinner" } });
            }

            Debug.WriteLine("url");
            Debug.WriteLine(requestUrl);

            var response = await httpClient.GetAsync(requestUrl);

            RecipeResponse result = new RecipeResponse();
            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<RecipeResponse>();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("content");
                Debug.WriteLine(content);

                foreach (RecipeHits hit in result.Hits)
                {
                    Debug.WriteLine(hit.Recipe.Label);
                    hit.Recipe.IsFavourite = favouritesDBService.IsFavourite(hit.Recipe.Url);
                }
            }

            return result.Hits;
        }
    }
}
