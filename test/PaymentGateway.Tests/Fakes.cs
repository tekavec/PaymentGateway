using System;
using LaYumba.Functional;
using PaymentGateway.Models;
using static LaYumba.Functional.F;

namespace PaymentGateway.Tests
{
    public class Fakes
    {
        public static Option<PaymentDetails> CreatePaymentDetails(Guid paymentId) =>
            Some(
                new PaymentDetails(
                    paymentId,
                    "a holder",
                    "a card number",
                    2029,
                    12,
                    "Success",
                    DateTime.Now));

    }
}