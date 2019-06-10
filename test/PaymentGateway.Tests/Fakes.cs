﻿using System;
using Acquirer.Client.Domain;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;

namespace PaymentGateway.Tests
{
    public class Fakes
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

        public static ProcessedPayment CreateNewPayment()
            => new ProcessedPayment(
                cardHolder: "a card holder",
                cardNumber: "a card number",
                expiryYear: 2029,
                expiryMonth: 12,
                amount: 12345.67m,
                currency: "GBP",
                acquirerPaymentId: Guid.NewGuid(),
                processedStatus: true);

        public static CreatePayment GetCreatePayment()
            => new CreatePayment(
                cardHolder: "a card holder",
                cardNumber: "a card number",
                cvv: "123",
                expiryYear: 2029,
                expiryMonth: 12,
                amount: 12345.67m,
                currency: "GBP");
    }
}