using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using CreditCardApi.Models;
using CreditCardApi.Repositories;
using CreditCardApi.Utils; // Certifique-se de que este namespace está sendo usado

namespace CreditCardApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerRepository _repository;

        public CustomerController(CustomerRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public IEnumerable<Customer> Get() => _repository.GetAllCustomers();

        [HttpGet("{id}")]
        public ActionResult<Customer> Get(int id)
        {
            var customer = _repository.GetCustomerById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }

        [HttpPost]
        public ActionResult<Customer> CreateCustomer([FromBody] CustomerCreateDto customerDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = new Customer
            {
                Name = customerDto.Name
            };

            _repository.AddCustomer(customer);
            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }

        [HttpPost("{customerId}/creditcards")]
        public ActionResult<CreditCard> AddCreditCard(int customerId, [FromBody] CreditCard creditCard)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var customer = _repository.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var existingCard = customer.Wallet.CreditCards.FirstOrDefault(c => c.CardNumber == creditCard.CardNumber);
            if (existingCard != null)
            {
                return Conflict("A card with the same number already exists.");
            }

            _repository.AddCreditCard(customerId, creditCard);
            return CreatedAtAction(nameof(Get), new { id = customerId, cardId = creditCard.Id }, creditCard);
        }

        [HttpPost("createWithCards")]
        public ActionResult<Customer> CreateCustomerWithCards([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Gera automaticamente o ID do cliente e da carteira
            customer.Id = 0;
            customer.Wallet.Id = 0;

            foreach (var card in customer.Wallet.CreditCards)
            {
                card.Id = 0;
                card.LastFourDigits = card.CardNumber.Substring(card.CardNumber.Length - 4);
                card.Token = HashHelper.GenerateMD5Hash(card.CardNumber + card.Cvv);
                card.CardNumber = string.Empty;  // Limpa o número do cartão original
                card.Cvv = string.Empty;  // Limpa o CVV original
            }

            _repository.AddCustomer(customer);
            return CreatedAtAction(nameof(Get), new { id = customer.Id }, customer);
        }

        [HttpDelete("{customerId}/creditcards/{cardId}")]
        public IActionResult RemoveCreditCard(int customerId, int cardId)
        {
            var customer = _repository.GetCustomerById(customerId);
            if (customer == null)
            {
                return NotFound();
            }

            var card = customer.Wallet.CreditCards.FirstOrDefault(c => c.Id == cardId);
            if (card == null)
            {
                return NotFound();
            }

            _repository.RemoveCreditCard(customerId, cardId);
            return NoContent();
        }
    }
}
