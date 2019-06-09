using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;

namespace PaymentGateway.Controllers
{
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
        public async Task<IActionResult> ProcessPayment([FromBody] MakePaymentV1 model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var paymentProcessingResult = await processPaymentService.Process(model);

            return new CreatedResult("get", paymentProcessingResult.Key);
        }

        [HttpGet]
        public async Task<IActionResult> GetPaymentDetails(Guid id)
        {
            return Ok(await retrievePaymentService.Get(id));
        }
    }
}