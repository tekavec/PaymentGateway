namespace PaymentGateway.Models
{
    public sealed class MakePaymentV1
    {
        public string CardNumber { get; set; }
        public string Cvv { get; set; }
        public int? ExpiryYear { get; set; }
        public int? ExpiryMonth { get; set; }
        public decimal Amount { get; set; }
        public string Currency { get; set; }
    }
}