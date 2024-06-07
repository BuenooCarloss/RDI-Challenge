using System.ComponentModel.DataAnnotations;

namespace CreditCardApi.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Customer name cannot exceed 50 characters.")]
        public string Name { get; set; } = string.Empty;

        public Wallet Wallet { get; set; } = new Wallet();
    }
}
