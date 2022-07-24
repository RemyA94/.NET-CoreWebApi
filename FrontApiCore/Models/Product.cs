using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace FrontApiCore.Models
{
    [JsonObject]
    public class Product
    {
        [JsonProperty("idProduct")]
        public int IdProduct { get; set; }

        [JsonProperty("barcode")]
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(maximumLength: 8, MinimumLength = 8, ErrorMessage = "The {0} field must contain 8 digits..")]
        public string Barcode { get; set; }

        [JsonProperty("name")]
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(maximumLength:50, MinimumLength = 2, ErrorMessage = "The length of the field {0} must be between {1} and {2}.")]
        public string Name { get; set; }

        [JsonProperty("brand")]
        [Required(ErrorMessage = "Field {0} is required")]
        [StringLength(maximumLength: 20, MinimumLength = 2, ErrorMessage = "The length of the field {0} must be between {1} and {2}.")]
        public string Brand { get; set; }

        [JsonProperty("category")]
        [Required(ErrorMessage = "Field {0} is required")]
        public string Category { get; set; }

        [JsonProperty("price")]
        [Required(ErrorMessage = "Field {0} is required")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$")]
        [Range(maximum: 9999999999999999.99, minimum: 1, ErrorMessage = "The length of the field {0} must be between {1} and {2}.")]
        public decimal Price { get; set; }

        
    }
}
