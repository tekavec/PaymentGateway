using System;

namespace PaymentGateway.Models
{
    public sealed class PaymentDetails
    {
        public PaymentDetails(
            Guid key,
            string cardHolder,
            string cardNumber,
            int expiryYear,
            int expiryMonth,
            bool processedStatus)
        {
            Key = key;
            CardHolder = cardHolder;
            CardNumber = cardNumber;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            ProcessedStatus = processedStatus;
        }

        public Guid Key { get; }
        public string CardHolder { get; }
        public string CardNumber { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public bool ProcessedStatus { get; }
    }
}