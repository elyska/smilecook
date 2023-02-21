using SQLite;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    [Table("myRecipes")]
    public class MyRecipe
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Label { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }

    }
}
