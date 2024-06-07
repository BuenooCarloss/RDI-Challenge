using System.ComponentModel.DataAnnotations;

namespace CreditCardApi.Models
{
    public class CreditCard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(16, MinimumLength = 13, ErrorMessage = "Card number must be between 13 and 16 digits.")]
        public string CardNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50, ErrorMessage = "Card holder name cannot exceed 50 characters.")]
        public string CardHolderName { get; set; } = string.Empty;

        [Required]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/[0-9]{4}$", ErrorMessage = "Expiry date must be in MM/yyyy format.")]
        public string ExpiryDate { get; set; } = string.Empty;

        [Required]
        [StringLength(4, MinimumLength = 3, ErrorMessage = "CVV must be between 3 and 4 digits.")]
        public string Cvv { get; set; } = string.Empty;

        // Novos campos para armazenar o token e os últimos quatro dígitos
        public string Token { get; set; } = string.Empty;
        public string LastFourDigits { get; set; } = string.Empty;
    }
}
