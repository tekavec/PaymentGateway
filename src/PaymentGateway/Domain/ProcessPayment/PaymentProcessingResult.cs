using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public struct PaymentProcessingResult
    {
        public PaymentProcessingResult(Guid key, PaymentProcessStatus status)
        {
            Key = key;
            Status = status;
        }

        public Guid Key { get; }
        public PaymentProcessStatus Status { get; }
    }
}