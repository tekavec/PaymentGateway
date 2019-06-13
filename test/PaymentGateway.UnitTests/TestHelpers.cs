using System;
using Acquirer.Client.Domain;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;

namespace PaymentGateway.UnitTests
{
    public class TestHelpers
    {
        public static PaymentDetails CreatePaymentDetails(Guid paymentId)
            => new PaymentDetails(
                paymentId,
                "a card holder",
                "a card number",
                2029,
                12,
                true);

        public static PaymentProcessingResult CreateSuccessfulPaymentProcessingResult(Guid key)
            => new PaymentProcessingResult(key, true);

        public static PaymentProcessingResult CreateUnsuccessfulPaymentProcessingResult(Guid key)
            => new PaymentProcessingResult(key, false);

        public static AcquirerProcessingResult CreateSuccessfulAcquirerProcessingResult(Guid acquirerPaymentId)
            => new AcquirerProcessingResult(acquirerPaymentId, true);

        public static AcquirerProcessingResult CreateUnsuccessfulAcquirerProcessingResult(Guid acquirerPaymentId)
            => new AcquirerProcessingResult(acquirerPaymentId, false);

        public static ProcessedPayment GetNewPayment()
            => new ProcessedPayment(
                cardHolder: "a card holder",
                cardNumber: "1234567890123456",
                expiryYear: 2029,
                expiryMonth: 12,
                amount: 12345.67m,
                currency: "GBP",
                acquirerPaymentId: Guid.NewGuid(),
                processedStatus: true);

        public static CreatePayment GetCreatePayment()
            => new CreatePayment(
                cardHolder: "a card holder",
                cardNumber: "1234567890123456",
                cvv: "123",
                expiryYear: 2029,
                expiryMonth: 12,
                amount: 12345.67m,
                currency: "GBP");

        public static MakePaymentV1 GetValidMakePaymentV1()
            => new MakePaymentV1
            {
                CardHolder = "a card holder",
                CardNumber = "12345678",
                Cvv = "123",
                ExpiryYear = DateTime.Today.Year + 1,
                ExpiryMonth = 12,
                Amount = 99.99m,
                Currency = "GBP"
            };
    }

    public class TestControllerContext : ControllerContext
    {
        public TestControllerContext()
        {
            HttpContext = new DefaultHttpContext();
        }
    }
}