using System;
using System.Threading.Tasks;
using Acquirer.Client.Domain;
using LaYumba.Functional;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;
using PaymentGateway.Validation;

namespace PaymentGateway.Controllers
{
    [Authorize]
    [ApiController]
    [Route("payment")]
    public class PaymentController : ControllerBase
    {
        private readonly IProcessPaymentService processPaymentService;
        private readonly IRetrievePaymentService retrievePaymentService;
        private readonly ILogger<PaymentController> logger;

        public PaymentController(
            IProcessPaymentService processPaymentService,
            IRetrievePaymentService retrievePaymentService,
            ILogger<PaymentController> logger)
        {
            this.processPaymentService = processPaymentService;
            this.retrievePaymentService = retrievePaymentService;
            this.logger = logger;
        }

        /// <summary>
        /// Process a new payment.
        /// </summary>
        /// <param name="command">Payment data</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(PaymentProcessingResult), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Post([FromBody] MakePaymentV1 command)
        {
            logger.LogInformation("Start processing new payment.");
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var processingResult = await ProcessPayment(command)
                .Map(
                    Faulted: ex =>
                    {
                        logger.LogError(ex, Errors.UnexpectedError.Message);
                        return StatusCode(500, Errors.UnexpectedError);
                    },
                    Completed: result => CreatedAtAction("Get", new { id = result.Key}, new PaymentProcessingResult(result.Key, result.IsPaymentSuccessful)));
            logger.LogInformation("Exit processing new payment.");
            return processingResult;
        }

        /// <summary>
        /// Retrieve details about the previously processed payment.
        /// </summary>
        /// <param name="id">Unique identifier of previously processed payment.</param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(PaymentDetails), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(BadRequestResult), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(UnauthorizedResult), StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(typeof(NotFoundResult), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Get(Guid id)
        {
            logger.LogInformation("Start retrieving payment details for payment id={id}.", id);
            var paymentDetails = await retrievePaymentService.Get(id)
                .Map(
                    Faulted: ex =>
                    {
                        logger.LogError(ex, Errors.UnexpectedError.Message);
                        return StatusCode(500, Errors.UnexpectedError);
                    },
                    Completed: val => val.Match(
                        Some: Ok,
                        None: () =>
                        {
                            logger.LogInformation("Payment details for id={id} not found.", id);
                            return NotFound($"Payment details for id={id} not found.") as ObjectResult;
                        }));

            logger.LogInformation("Exit retrieving payment details for payment id={id}.", id);
            return paymentDetails;
        }

        private Task<PaymentProcessingResult> ProcessPayment(MakePaymentV1 command)
        {
            return processPaymentService.Process(GetCreatePayment(command));
        }

        private static CreatePayment GetCreatePayment(MakePaymentV1 model)
        {
            return new CreatePayment(
                model.CardNumber,
                model.Cvv,
                (int)model.ExpiryYear,
                (int)model.ExpiryMonth,
                model.Amount,
                model.Currency);
        }
    }
}