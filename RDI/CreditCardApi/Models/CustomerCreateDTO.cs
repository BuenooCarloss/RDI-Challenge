using System.ComponentModel.DataAnnotations;

namespace CreditCardApi.Models
{
    public class CustomerCreateDto
    {
        [Required]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;
    }
}
