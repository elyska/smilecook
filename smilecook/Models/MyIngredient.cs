using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    [Table("myIngredients")]
    public class MyIngredient
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string IngredientLine { get; set; }
    }
}
