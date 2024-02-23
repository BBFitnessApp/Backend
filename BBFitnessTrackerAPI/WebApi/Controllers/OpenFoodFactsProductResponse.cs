namespace WebApi.Controllers
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class OpenFoodFactsProductResponse
    {
        public Nutriments ?Nutriments { get; set; }
    }

    public class Nutriments
    {
         public double Kalorien { get; set; }

         public double Fette { get; set; }

         public double Proteine { get; set; }

         public double Kohlenhydrate { get; set; }

         public string Name { get; set; } = string.Empty;
    }
}