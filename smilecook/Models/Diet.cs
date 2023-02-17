using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    [Table("diets")]
    public class Diet
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(30), Unique]
        public string Name { get; set; }
        public bool IsSelected { get; set; }
    }
}
