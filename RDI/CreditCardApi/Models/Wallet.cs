using System.Collections.Generic;

namespace CreditCardApi.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public List<CreditCard> CreditCards { get; set; } = new List<CreditCard>();
    }
}
