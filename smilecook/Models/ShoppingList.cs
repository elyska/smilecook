using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    [Table("shoppingList")]
    public class ShoppingList
    {

        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250)]
        public string Ingredient { get; set; }
    }
}
