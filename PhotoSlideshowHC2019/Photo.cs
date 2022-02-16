using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoSlideshowHC2019
{
    internal class Photo
    {
        public int Id { get; set; }
        public char Orientamento { get; set; }
        public int NumberOfTags { get; set; }
        public List<string> tags { get; set; }
    }
}
