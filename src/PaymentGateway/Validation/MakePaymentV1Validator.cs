using System;
using FluentValidation;
using PaymentGateway.Models;

namespace PaymentGateway.Validation
{
    public class MakePaymentV1Validator : AbstractValidator<MakePaymentV1>
    {
        private const int DefinitelyExpiredYear = 1900;
        private const int FirstMonthOfYear = 1; 
        private const int FirstDayOfMonth = 1; 

        public MakePaymentV1Validator(Func<DateTime> clock)
        {
            RuleFor(x => x.CardHolder)
                .NotEmpty()
                .Matches("^.{1,161}$")
                .WithName(c => nameof(c.CardHolder))
                .WithMessage("Must have 1 to 161 characters.");

            RuleFor(x => x.CardNumber)
                .NotEmpty()
                .Matches("^\\d{8,19}$")
                .WithName(c => nameof(c.CardNumber))
                .WithMessage("Must contain 8 to 19 digits.");

            RuleFor(x => x.Cvv)
                .NotEmpty()
                .Matches("^\\d{3,4}$")
                .WithName(c => nameof(c.Cvv))
                .WithMessage("Must contain 3 to 4 digits.");

            RuleFor(x => x.Currency)
                .NotEmpty()
                .Matches("^[a-zA-Z]{3}$")
                .WithName(c => nameof(c.Currency))
                .WithMessage(c => "Must contain three letters.");

            RuleFor(x => x.ExpiryYear)
                .NotEmpty()
                .GreaterThanOrEqualTo(clock.Invoke().Year)
                .Must((x, year) => NotBeExpired(clock, year, x.ExpiryMonth))
                .WithMessage(c => "Credit card has expired.");

            RuleFor(x => x.ExpiryMonth)
                .NotEmpty()
                .InclusiveBetween(1, 12)
                .WithName(c => nameof(c.ExpiryMonth))
                .WithMessage(c => "Must be between 1 and 12.");
        }

        private bool NotBeExpired(Func<DateTime> clock, int? expiryYear, int? expiryMonth)
        {
            if (expiryMonth == null || expiryMonth < 1 || expiryMonth > 12)
                return false;
            var now = clock.Invoke();
            var expiryDate = new DateTime(
                    expiryYear ?? DefinitelyExpiredYear, 
                    expiryMonth ?? FirstMonthOfYear, 
                    FirstDayOfMonth)
                .AddMonths(1).AddDays(-1);
            return now <= expiryDate;
        }
    }
}