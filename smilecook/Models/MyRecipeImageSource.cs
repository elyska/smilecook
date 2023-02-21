using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace smilecook.Models
{
    public class MyRecipeImageSource
    {
        public int Id { get; set; }
        public string Label { get; set; }
        public string Image { get; set; }
        public string Instructions { get; set; }
        public ImageSource ImgSource { get; set; }
    }
}
