using System;
using System.Threading.Tasks;
using LaYumba.Functional;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.RetrievePayment
{
    public class RetrievePaymentService : IRetrievePaymentService
    {
        private readonly IReadPaymentRepository readPaymentRepository;

        public RetrievePaymentService(IReadPaymentRepository readPaymentRepository)
        {
            this.readPaymentRepository = readPaymentRepository;
        }

        public async Task<Option<PaymentDetails>> Get(Guid id)
        {
            return await readPaymentRepository.Read(id);
        }
    }
}