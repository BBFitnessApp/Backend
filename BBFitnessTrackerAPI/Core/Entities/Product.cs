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
        public string Barcode { get; set; } = string.Empty; // Barcode aka Spoontacular ID 
        public string Produktname { get; set; } = string.Empty; // Produktname (String)
        public double Kalorien { get; set; } // Kalorien (Integer)
        public double Fette { get; set; } // Fette (Integer)
        public double Proteine { get; set; }
        public double Kohlenhydrate { get; set; }
    }
}
