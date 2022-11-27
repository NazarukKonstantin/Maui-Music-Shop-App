using COURSE_ASH.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace COURSE_ASH.Services
{
    public static class TempServer
    {
        private readonly static Product[] _products;

        private readonly static ObservableCollection<Product> _productCollection = new()
        {
            new("Guitar", "lol_1", "Good guitar", 132.42M, "guitar_catalog.png", 3),
            new("MIDI", "Model_1", "Good midi", 164.3423M, "midi_catalog.png", 5),
            new("Sax", "lol_2", "Good sax", 562.42M, "sax_catalog.png", 2),
            new("Ukulele", "Model_1", "Good ukulele", 32.42M, "ukulele_catalog.png", 1),
            new("Violin", "lol_3", "Good violin", 322.42M, "violin_catalog.png", 4),
            new("Guitar Accessory", "Model_1", "Good shit", 232.42M, "guitar_accessories_catalog.png", 0)
        };

        public static ObservableCollection<Product> GetProducts()
        {
            return _productCollection;
        }
    }
}
