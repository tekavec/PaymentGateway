using System;
using System.Threading.Tasks;
using LaYumba.Functional;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Models;

namespace PaymentGateway.Domain.RetrievePayment
{
    public class RetrievePaymentService : IRetrievePaymentService
    {
        private readonly IReadPaymentRepository readPaymentRepository;
        private readonly ILogger<RetrievePaymentService> logger;

        public RetrievePaymentService(
            IReadPaymentRepository readPaymentRepository,
            ILogger<RetrievePaymentService> logger)
        {
            this.readPaymentRepository = readPaymentRepository;
            this.logger = logger;
        }

        public async Task<Option<PaymentDetails>> Get(Guid id)
        {
            logger.LogInformation("Start retrieving payment details for id={id}.", id);
            var paymentDetails = await readPaymentRepository.Read(id);
            logger.LogInformation("Exit retrieving payment details for id={id}.", id);
            return paymentDetails;
        }
    }
}