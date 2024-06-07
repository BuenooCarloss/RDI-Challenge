using System.Collections.Generic;
using System.Linq;
using CreditCardApi.Models;
using CreditCardApi.Utils;

namespace CreditCardApi.Repositories
{
    public class CustomerRepository
    {
        private readonly List<Customer> _customers = new List<Customer>();
        private int _nextCustomerId = 1;
        private int _nextWalletId = 1;
        private int _nextCardId = 1;

        public IEnumerable<Customer> GetAllCustomers() => _customers;

        public Customer? GetCustomerById(int id) => _customers.FirstOrDefault(c => c.Id == id);

        public void AddCustomer(Customer customer)
        {
            customer.Id = _nextCustomerId++;
            customer.Wallet.Id = _nextWalletId++;
            _customers.Add(customer);
        }

        public void AddCreditCard(int customerId, CreditCard creditCard)
        {
            var customer = GetCustomerById(customerId);
            if (customer != null)
            {
                creditCard.Id = _nextCardId++;
                creditCard.LastFourDigits = creditCard.CardNumber.Substring(creditCard.CardNumber.Length - 4);
                creditCard.Token = HashHelper.GenerateMD5Hash(creditCard.CardNumber + creditCard.Cvv);
                creditCard.CardNumber = string.Empty;  // Limpa o número do cartão original
                creditCard.Cvv = string.Empty;  // Limpa o CVV original
                customer.Wallet.CreditCards.Add(creditCard);
            }
        }

        public void RemoveCreditCard(int customerId, int cardId)
        {
            var customer = GetCustomerById(customerId);
            if (customer != null)
            {
                var card = customer.Wallet.CreditCards.FirstOrDefault(c => c.Id == cardId);
                if (card != null)
                {
                    customer.Wallet.CreditCards.Remove(card);
                }
            }
        }
    }
}
