namespace PaymentGateway.Models
{
    public static class StringExtensions
    {
        private const string FixedMaskedPrefix = "******";

        public static string MaskAllExceptLast(this string self, int numberOfCharacters)
        {
            if (string.IsNullOrEmpty(self) || numberOfCharacters >= self.Length)
            {
                return self;
            }

            return FixedMaskedPrefix + self.Substring(self.Length - numberOfCharacters, numberOfCharacters);
        }
    }
}