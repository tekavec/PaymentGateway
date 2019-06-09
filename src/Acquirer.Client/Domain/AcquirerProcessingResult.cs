using System;

namespace Acquirer.Client.Domain
{
    public sealed class AcquirerProcessingResult
    {
        public AcquirerProcessingResult(
            Guid acquirerPaymentId,
            bool isPaymentSuccessful)
        {
            AcquirerPaymentId = acquirerPaymentId;
            IsPaymentSuccessful = isPaymentSuccessful;
        }

        public Guid AcquirerPaymentId { get; }
        public bool IsPaymentSuccessful { get; }
    }

}