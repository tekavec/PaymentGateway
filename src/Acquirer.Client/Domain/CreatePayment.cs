namespace Acquirer.Client.Domain
{
    public sealed class CreatePayment
    {
        public CreatePayment(
            string cardNumber,
            string cvv,
            int expiryYear,
            int expiryMonth,
            decimal amount,
            string currency)
        {
            CardNumber = cardNumber;
            Cvv = cvv;
            ExpiryYear = expiryYear;
            ExpiryMonth = expiryMonth;
            Amount = amount;
            Currency = currency;
        }

        public string CardNumber { get; }
        public string Cvv { get; }
        public int ExpiryYear { get; }
        public int ExpiryMonth { get; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}