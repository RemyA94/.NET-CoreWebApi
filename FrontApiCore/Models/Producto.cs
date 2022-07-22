using Newtonsoft.Json;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class Producto
    {
        [JsonProperty("idProducto")]
        public int IdProducto { get; set; }

        [JsonProperty("codigoBarra")]
        public string CodigoBarra { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("marca")]
        public string Marca { get; set; }

        [JsonProperty("categoria")]
        public string Categoria { get; set; }

        [JsonProperty("precio")]
        public decimal Precio { get; set; }

        //[JsonProperty("categoria")]
        //public virtual Categoria oCategoria { get; set; }
    }
}
