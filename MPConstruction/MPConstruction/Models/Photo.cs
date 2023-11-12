using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace MPConstruction.Models
{
    class Photo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Source { get; set; }
        public FileResult Ref { get; set; }
    }
}
