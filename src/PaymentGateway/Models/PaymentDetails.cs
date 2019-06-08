using System;

namespace PaymentGateway.Models
{
    public sealed class PaymentDetails
    {
        public PaymentDetails(
            string cardHolder,
            string cardNumber,
            int expiryYear,
            int expiryMonth,
            string processedStatus,
            DateTime processedAt)
        {
            CardHolder = cardHolder;
            CardNumber = cardNumber;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            ProcessedStatus = processedStatus;
            ProcessedAt = processedAt;
        }

        public string CardHolder { get; }
        public string CardNumber { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public string ProcessedStatus { get; }
        public DateTime ProcessedAt { get; }
    }
}