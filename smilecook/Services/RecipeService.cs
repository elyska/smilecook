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
    public class RecipeService
    {
        HttpClient httpClient;

        string url = "https://api.edamam.com/api/recipes/v2";
        Dictionary<string, string> queryParams = new Dictionary<string, string>() // default query parameters
        {
            {"app_id", "a4aac7a2" },
            {"app_key", "fc46385a390444b36f97eb6c55e833b8" },
            {"type","public" },
        };

        public RecipeService()
        {
            httpClient = new HttpClient();

            url = QueryHelpers.AddQueryString(url, queryParams); // add default query parameters
        }

        public async Task<List<RecipeHits>> GetRecipes() // get random recipes
        {
            RecipeResponse result = new RecipeResponse();

            var optionalQueryParams = new Dictionary<string, string>() 
            {
                {"mealType", "Dinner" } // change depending on the time of the day
            };
            string requestUrl = QueryHelpers.AddQueryString(url, optionalQueryParams);

            Debug.WriteLine("url");
            Debug.WriteLine(requestUrl);

            var response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<RecipeResponse>();
                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("content");
                Debug.WriteLine(content);
            }

            return result.Hits;
        }

        public async Task<List<RecipeHits>> SearchByName(string searchTerm, List<Dictionary<string, string>> filters) // search for specific recipes
        {
            RecipeResponse result = new RecipeResponse();

            // add search term to query params
            var optionalQueryParams = new Dictionary<string, string>()
            {
                {"q", searchTerm }
            };
            string requestUrl = QueryHelpers.AddQueryString(url, optionalQueryParams);

            // add filters to query params
            foreach (var filter in filters)
            {
                requestUrl = QueryHelpers.AddQueryString(requestUrl, filter);
            }

            Debug.WriteLine("url");
            Debug.WriteLine(requestUrl);

            var response = await httpClient.GetAsync(requestUrl);

            if (response.IsSuccessStatusCode)
            {
                result = await response.Content.ReadFromJsonAsync<RecipeResponse>();

                var content = await response.Content.ReadAsStringAsync();
                Debug.WriteLine("content");
                Debug.WriteLine(content);
            }

            return result.Hits;
        }
    }
}
