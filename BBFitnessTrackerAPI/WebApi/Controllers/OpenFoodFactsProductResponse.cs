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
        [JsonProperty("energy-kcal_value")]
        public double Kalorien { get; set; }

        [JsonProperty("fat")]
        public double Fette { get; set; }

        [JsonProperty("proteins")]
        public double Proteine { get; set; }

        [JsonProperty("carbohydrates")]
        public double Kohlenhydrate { get; set; }

        [JsonProperty("product_name")]
        public string Name { get; set; } = string.Empty;
    }
}