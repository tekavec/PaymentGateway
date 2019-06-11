using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public sealed class PaymentProcessingResult
    {
        public PaymentProcessingResult(Guid key, bool status)
        {
            Key = key;
            Status = status;
        }

        public Guid Key { get; }
        public bool Status { get; }
    }
}