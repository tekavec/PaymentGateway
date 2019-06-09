using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public struct PaymentProcessingResult
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