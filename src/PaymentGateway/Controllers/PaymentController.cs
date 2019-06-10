using System;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IProcessPaymentService processPaymentService;
        private readonly IRetrievePaymentService retrievePaymentService;

        public PaymentController(IProcessPaymentService processPaymentService, IRetrievePaymentService retrievePaymentService)
        {
            this.processPaymentService = processPaymentService;
            this.retrievePaymentService = retrievePaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MakePaymentV1 model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var createPayment = new CreatePayment(
                model.CardHolder,
                model.CardNumber,
                model.Cvv,
                model.ExpiryYear,
                model.ExpiryMonth,
                model.Amount,
                model.Currency);
            var paymentProcessingResult = await processPaymentService.Process(createPayment);

            return new CreatedResult("get", paymentProcessingResult.Key);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await retrievePaymentService.Get(id));
        }
    }
}