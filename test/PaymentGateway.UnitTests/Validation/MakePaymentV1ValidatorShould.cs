using System;
using FluentValidation.TestHelper;
using PaymentGateway.Models;
using PaymentGateway.Validation;
using Xunit;

namespace PaymentGateway.UnitTests.Validation
{
    public class MakePaymentV1ValidatorShould
    {
        private readonly MakePaymentV1Validator validator;
        public MakePaymentV1ValidatorShould()
        {
            validator = new MakePaymentV1Validator(() => DateTime.Now);
        }

        [Theory]
        [InlineData(null, "Null is invalid card holder")]
        [InlineData("", "Empty string is invalid card holder")]
        public void validate_invalid_card_holder(string cardHolder, string because)
        {
            validator.ShouldHaveValidationErrorFor(a => a.CardHolder, cardHolder);
        }

        [Fact]
        public void validate_too_long_card_holder()
        {
            validator.ShouldHaveValidationErrorFor(a => a.CardHolder, new string('a', 162));
        }

        [Theory]
        [InlineData("a")]
        [InlineData("0")]
        public void validate_valid_card_holder(string cardHolder)
        {
            validator.ShouldNotHaveValidationErrorFor(a => a.CardHolder, cardHolder);
        }

        [Theory]
        [InlineData(null, "Null is invalid card number")]
        [InlineData("", "Empty string is invalid card number")]
        [InlineData("1234567", "Less than 8 digits is invalid card number")]
        [InlineData("12345678901234567890", "More than 19 digits is invalid card number")]
        [InlineData("12345678A", "Non-digit characters are not allowed for card number")]
        public void validate_invalid_card_number(string cardNumber, string because)
        {
            validator.ShouldHaveValidationErrorFor(a => a.CardNumber, cardNumber);
        }

        [Theory]
        [InlineData("12345678")]
        [InlineData("01234567")]
        [InlineData("1234567890123456789")]
        public void validate_valid_card_number(string cardNumber)
        {
            validator.ShouldNotHaveValidationErrorFor(a => a.CardNumber, cardNumber);
        }

        [Theory]
        [InlineData(null, "Null is invalid CVV")]
        [InlineData("", "Empty string is invalid CVV")]
        [InlineData("12", "Less than 3 digits is invalid CVV")]
        [InlineData("12345", "More than 4 digits is invalid CVV")]
        [InlineData("12345678A", "Non-digit characters are not allowed for card number")]
        public void validate_invalid_cvv(string cvv, string because)
        {
            validator.ShouldHaveValidationErrorFor(a => a.Cvv, cvv);
        }

        [Theory]
        [InlineData("123")]
        [InlineData("012")]
        [InlineData("1234")]
        public void validate_valid_cvv(string cvv)
        {
            validator.ShouldNotHaveValidationErrorFor(a => a.Cvv, cvv);
        }

        [Fact]
        public void validate_expired_card()
        {
            var expiryValidator = new MakePaymentV1Validator(() => new DateTime(2019, 6, 1));
            expiryValidator.ShouldHaveValidationErrorFor(a => a.ExpiryYear,
                new MakePaymentV1 {ExpiryYear = 2019, ExpiryMonth = 5});
        }

        [Fact]
        public void validate_not_expired_card()
        {
            var expiryValidator = new MakePaymentV1Validator(() => new DateTime(2019, 5, 31));
            expiryValidator.ShouldNotHaveValidationErrorFor(a => a.ExpiryYear,
                new MakePaymentV1 {ExpiryYear = 2019, ExpiryMonth = 5});
        }

        [Theory]
        [InlineData(null)]
        [InlineData(0)]
        [InlineData(13)]
        public void validate_invalid_expiry_month(int? month)
        {
            validator.ShouldHaveValidationErrorFor(a => a.ExpiryMonth, month);
        }

        [Fact]
        public void validate_valid_expiry_month()
        {
            for (var i = 1; i <= 12; i++)
            {
                validator.ShouldNotHaveValidationErrorFor(a => a.ExpiryMonth, i);
            }
        }
    }
}