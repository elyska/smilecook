using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    [Table("favourites")]
    public class Favourite
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(250), Unique]
        public string Url { get; set; }
        public string Label { get; set; }

        [MaxLength(250)]
        public string Image { get; set; }
    }
}
