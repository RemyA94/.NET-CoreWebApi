using System.ComponentModel.DataAnnotations;

namespace FrontApiCore.Models
{
    public class Credential
    {
        [EmailAddress]
        [Required(ErrorMessage = "Field {0} is required")]
        public string Mail { get; set; }

        [Key]
        [Required(ErrorMessage = "Field {0} is required")]
        public string Key { get; set; }
    }
}
