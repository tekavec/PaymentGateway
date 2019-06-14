using FluentAssertions;
using PaymentGateway.Models;
using Xunit;

namespace PaymentGateway.UnitTests.Models
{
    public class StringExtensionsShould
    {
        [Theory]
        [InlineData(null, 4, null)]
        [InlineData("", 4, "")]
        [InlineData("abc", 4, "abc")]
        [InlineData("abcd", 4, "abcd")]
        [InlineData("abcde", 4, "******bcde")]
        [InlineData("abcdefghij", 4, "******ghij")]
        public void mask_first_part_of_a_string(string input, int numberOfCharacters, string expectedResult)
        {
            input.MaskAllExceptLast(numberOfCharacters).Should().Be(expectedResult);
        }
    }
}