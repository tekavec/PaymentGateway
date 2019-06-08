﻿using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using PaymentGateway.Controllers;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using System;
using System.Threading.Tasks;
using PaymentGateway.Domain.RetrievePayment;
using Xunit;

namespace PaymentGateway.Tests.Controllers
{
    public class PaymentControllerShould
    {
        private readonly TestControllerContext controllerContext = new TestControllerContext();
        private readonly PaymentController paymentController;
        private readonly Mock<IProcessPaymentService> processPaymentService = new Mock<IProcessPaymentService>(MockBehavior.Strict);
        private readonly Mock<IRetrievePaymentService> retrievePaymentService = new Mock<IRetrievePaymentService>(MockBehavior.Strict);

        public PaymentControllerShould()
        {
            paymentController = new PaymentController(processPaymentService.Object, retrievePaymentService.Object)
            {
                ControllerContext = controllerContext
            };
        }

        [Fact]
        public async Task return_created_at_result_when_payment_successfully_processed()
        {
            processPaymentService.Setup(a => a.Process(It.IsAny<MakePaymentV1>()))
                .ReturnsAsync(PaymentProcessingResult.CreateSuccessfulResult(Guid.NewGuid()));

            var result = await paymentController.ProcessPayment(new MakePaymentV1());

            result.Should().BeOfType<CreatedResult>();
        }
        
        [Fact]
        public async Task return_result_of_processed_payment_when_payment_successfully_processed()
        {
            var transactionId = Guid.NewGuid();
            processPaymentService.Setup(a => a.Process(It.IsAny<MakePaymentV1>()))
                .ReturnsAsync(PaymentProcessingResult.CreateSuccessfulResult(transactionId));

            var result = await paymentController.ProcessPayment(new MakePaymentV1()) as CreatedResult;

            result.Value.Should().Be(transactionId);
        }
        
        [Fact]
        public async Task return_bad_request_for_invalid_input_payment_data()
        {
            controllerContext.ModelState.AddModelError("Request", "Invalid");

            var result = await paymentController.ProcessPayment(new MakePaymentV1());

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task return_ok_when_payment_successfully_retrieved()
        {
            retrievePaymentService.Setup(a => a.Get(It.IsAny<Guid>())).ReturnsAsync(It.IsAny<PaymentDetails>());

            var result = await paymentController.GetPaymentDetails(Guid.NewGuid());

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]

        public async Task return_payment_details_for_a_given_payment_identifier()
        {
            var paymentDetails = CreatePaymentDetails();
            retrievePaymentService.Setup(a => a.Get(It.IsAny<Guid>())).ReturnsAsync(paymentDetails);

            var result = await paymentController.GetPaymentDetails(Guid.NewGuid()) as OkObjectResult;

            result.Value.Should().BeOfType<PaymentDetails>();
            result.Value.Should().Be(paymentDetails);
        }

        private static PaymentDetails CreatePaymentDetails() =>
            new PaymentDetails("a holder", "a card number", 2029, 12, "Success", DateTime.Now);

    }
}