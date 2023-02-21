using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    public class RecipeDetails
    {
        public string Label { get; set; }
        public string Url { get; set; }
        public string Image { get; set; }
        public List<string> MealType { get; set; }
        public List<string> DietLabels { get; set; }
        public List<string> HealthLabels { get; set; }
        public List<string> IngredientLines { get; set; }
        public List<string> CuisineType { get; set; }
        public double Calories { get; set; }
        public double TotalTime { get; set; }
        public double TotalWeight { get; set; }
        public bool IsFavourite { get; set; }
    }
}
