using System;
using LaYumba.Functional;
using PaymentGateway.Domain;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using static LaYumba.Functional.F;

namespace PaymentGateway.Tests
{
    public class Fakes
    {
        public static Option<PaymentDetails> CreatePaymentDetails(Guid paymentId)
            => Some(
                new PaymentDetails(
                    paymentId,
                    "a card holder",
                    "a card number",
                    2029,
                    12,
                    "Success",
                    DateTime.Now));

        public static PaymentProcessingResult CreateSuccessfulPaymentProcessingResult(Guid key)
            => new PaymentProcessingResult(key, PaymentProcessStatus.Succeeded);

        public static PaymentProcessingResult CreateUnsuccessfulPaymentProcessingResult(Guid key)
            => new PaymentProcessingResult(key, PaymentProcessStatus.Failed);

        public static AcquirerProcessingResult CreateSuccessfulAcquirerProcessingResult(string acquirerPaymentId)
            => new AcquirerProcessingResult(acquirerPaymentId, PaymentProcessStatus.Succeeded, DateTime.Now);

        public static AcquirerProcessingResult CreateUnsuccessfulAcquirerProcessingResult(string acquirerPaymentId)
            => new AcquirerProcessingResult(acquirerPaymentId, PaymentProcessStatus.Failed, DateTime.Now);

        public static NewPayment CreateNewPayment()
            => new NewPayment(
                cardHolder: "a card holder",
                cardNumber: "a card number",
                expiryYear: 2029,
                expiryMonth: 12,
                amount: 12345.67m,
                currency: "GBP",
                acquirerPaymentId: Guid.NewGuid().ToString(),
                processedStatus: "Success",
                processedAt: DateTime.Now);
    }
}