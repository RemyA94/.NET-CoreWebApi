
using Newtonsoft.Json;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class FrontRequest
    {


        public string message { get; set; }


        public Product response { get; set; }
    }
}
