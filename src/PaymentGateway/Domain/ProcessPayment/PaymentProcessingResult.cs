using System;

namespace PaymentGateway.Domain.ProcessPayment
{
    public struct PaymentProcessingResult
    {
        private PaymentProcessingResult(Guid paymentId, PaymentProcessStatus status)
        {
            PaymentId = paymentId;
            Status = status;
        }

        public static PaymentProcessingResult CreateSuccessfulResult(Guid paymentId)
        {
            return new PaymentProcessingResult(paymentId, PaymentProcessStatus.Succeeded);
        }

        public static PaymentProcessingResult CreateFailedResult(Guid paymentId)
        {
            return new PaymentProcessingResult(paymentId, PaymentProcessStatus.Failed);
        }

        public Guid PaymentId { get; }
        public PaymentProcessStatus Status { get; }
    }
}