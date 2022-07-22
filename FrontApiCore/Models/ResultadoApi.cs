using Newtonsoft.Json;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class ResultadoApi
    {
        public string Mensaje { get; set; }           
        public List<Producto> response { get; set; }

    }
}
