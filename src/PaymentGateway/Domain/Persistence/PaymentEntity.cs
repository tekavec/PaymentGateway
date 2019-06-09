using System;

namespace PaymentGateway.Domain.Persistence
{
    public class PaymentEntity : Entity<Guid>
    {
        public PaymentEntity(
            Guid key,
            string cardHolder,
            string cardNumber,
            int expiryYear,
            int expiryMonth,
            decimal amount,
            string currency,
            string acquirerPaymentId,
            string processedStatus,
            DateTime processedAt)
        {
            Key = key;
            CardHolder = cardHolder;
            CardNumber = cardNumber;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            Amount = amount;
            Currency = currency;
            AcquirerPaymentId = acquirerPaymentId;
            ProcessedStatus = processedStatus;
            ProcessedAt = processedAt;
        }

        public override Guid Key { get; }
        public string CardHolder { get; }
        public string CardNumber { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string AcquirerPaymentId { get; }
        public string ProcessedStatus { get; }
        public DateTime ProcessedAt { get; }
    }
}