using Newtonsoft.Json;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class Product
    {
        [JsonProperty("idProducto")]
        public int IdProduct { get; set; }

        [JsonProperty("codigoBarra")]
        public string Barcode { get; set; }

        [JsonProperty("descripcion")]
        public string Name { get; set; }

        [JsonProperty("marca")]
        public string Brand { get; set; }

        [JsonProperty("categoria")]
        public string Category { get; set; }

        [JsonProperty("precio")]
        public decimal Price { get; set; }

        //[JsonProperty("categoria")]
        //public virtual Categoria oCategoria { get; set; }
    }
}
