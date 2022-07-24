using Newtonsoft.Json;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class ApiResponse
    {

        public string message { get; set; }
        public List<Product> response { get; set; }

    }
}
