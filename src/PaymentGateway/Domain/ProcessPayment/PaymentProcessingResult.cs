using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public sealed class PaymentProcessingResult
    {
        public PaymentProcessingResult(Guid key, bool isPaymentSuccessful)
        {
            Key = key;
            IsPaymentSuccessful = isPaymentSuccessful;
        }

        public Guid Key { get; }
        public bool IsPaymentSuccessful { get; }
    }
}