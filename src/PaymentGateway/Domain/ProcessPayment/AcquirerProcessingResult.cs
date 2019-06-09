using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public struct AcquirerProcessingResult
    {
        public AcquirerProcessingResult(
            string acquirerPaymentId, 
            PaymentProcessStatus status, 
            DateTime processedAt)
        {
            AcquirerPaymentId = acquirerPaymentId;
            Status = status;
            ProcessedAt = processedAt;
        }

        public string AcquirerPaymentId { get; }
        public PaymentProcessStatus Status { get; }
        public DateTime ProcessedAt { get; }
    }
}