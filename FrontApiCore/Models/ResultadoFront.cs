
using Newtonsoft.Json;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class ResultadoFront
    {

        public string mensaje { get; set; }

        public Producto response { get; set; }
    }
}
