using System;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using LaYumba.Functional;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;
using PaymentGateway.Validation;

namespace PaymentGateway.Controllers
{
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IProcessPaymentService processPaymentService;
        private readonly IRetrievePaymentService retrievePaymentService;

        public PaymentController(
            IProcessPaymentService processPaymentService,
            IRetrievePaymentService retrievePaymentService)
        {
            this.processPaymentService = processPaymentService;
            this.retrievePaymentService = retrievePaymentService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] MakePaymentV1 command)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var objectResult = await ProcessPayment(command)
                .Map(
                    Faulted: ex => StatusCode(500, Errors.UnexpectedError),
                    Completed: result => CreatedAtAction("Get", new { id = result.Key}, new PaymentProcessingResult(result.Key, result.Status)));
            return objectResult;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
            => await retrievePaymentService.Get(id)
                .Map(
                    Faulted: ex => StatusCode(500, Errors.UnexpectedError),
                    Completed: val => val.Match(
                        Some: Ok, 
                        None: () => NotFound($"Payment details for id={id} not found.") as ObjectResult));

        private Task<PaymentProcessingResult> ProcessPayment(MakePaymentV1 command)
        {
            return processPaymentService.Process(GetCreatePayment(command));
        }

        private static CreatePayment GetCreatePayment(MakePaymentV1 model)
        {
            return new CreatePayment(
                model.CardHolder,
                model.CardNumber,
                model.Cvv,
                (int)model.ExpiryYear,
                (int)model.ExpiryMonth,
                model.Amount,
                model.Currency);
        }
    }
}