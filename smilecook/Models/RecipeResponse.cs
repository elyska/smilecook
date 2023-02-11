using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    class RecipeResponse
    {
        public int Count { get; set; }
        public List<RecipeHits> Hits { get; set; }
    }
}
