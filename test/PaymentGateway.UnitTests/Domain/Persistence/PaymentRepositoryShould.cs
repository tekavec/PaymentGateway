using System;
using System.Threading.Tasks;
using FluentAssertions;
using LaYumba.Functional;
using Moq;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.Persistence;
using Xunit;
using Xunit.Sdk;

namespace PaymentGateway.UnitTests.Domain.Persistence
{
    public class PaymentRepositoryShould
    {
        private readonly Mock<IIdentityGenerator<Guid>> identityGenerator = new Mock<IIdentityGenerator<Guid>>();
        private readonly PaymentRepository paymentRepository;

        public PaymentRepositoryShould()
        {
            paymentRepository = new PaymentRepository(identityGenerator.Object);
        }

        [Fact]
        public async Task retrieve_stored_payment_if_it_exist()
        {
            var id = Guid.NewGuid();
            identityGenerator.Setup(a => a.NewId).Returns(id);
            var newPayment = TestHelpers.GetNewPayment();
            await paymentRepository.Save(newPayment);

            var result = await paymentRepository.Read(id);

            result.Match(
                None: () => throw new XunitException("Result shouldn't be None."),
                Some: paymentDetails => paymentDetails.Key.Should().Be(id));
        }

        [Fact]
        public async Task does_not_retrieve_non_existing_payment()
        {
            var result = await paymentRepository.Read(Guid.NewGuid());

            result.Match(
                None: () => {},
                Some: _ => throw new XunitException("Result should be None."));
        }
    }
}