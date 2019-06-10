using System.Linq;
using FluentAssertions;
using PaymentGateway.Validators;
using Xunit;
using Xunit.Sdk;
using static PaymentGateway.Validators.ValidationStrategies;
using static LaYumba.Functional.F;

namespace PaymentGateway.Tests.Validators
{
    public class HarvestErrorsValidationStrategyShould
    {
        static readonly Validator<int> Success = Valid;
        static readonly Validator<int> Failure = _ => Error("Invalid");

        [Fact]
        public void succeed_when_all_validators_succeed() 
            => HarvestErrors(Success, Success)(1).Should().Be(Valid(1));

        [Fact]
        public void succeed_if_no_validators_are_used() 
            => HarvestErrors<int>()(1).Should().Be(Valid(1));

        [Fact]
        public void fail_when_at_least_one_validator_fails() 
            => HarvestErrors(Success, Failure)(1).Match(
                Valid: _ => throw new XunitException("Result shouldn't be Valid."),
                Invalid: errs => errs.Count().Should().Be(1));

        [Fact]
        public void WhenSeveralValidatorsFail_ThenFail() 
            => HarvestErrors(Success, Failure, Failure, Success)(1).Match(
                Valid: _ => throw new XunitException("Result shouldn't be Valid."),
                Invalid: (errs) => errs.Count().Should().Be(2));
    }
}