using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public string Barcode { get; set; } = string.Empty; // Barcode (String oder Integer, je nach Barcode-Format)
        public string Produktname { get; set; } = string.Empty; // Produktname (String)
        public int Kalorien { get; set; } // Kalorien (Integer)
        public int Fette { get; set; } // Fette (Integer)
    }
}
