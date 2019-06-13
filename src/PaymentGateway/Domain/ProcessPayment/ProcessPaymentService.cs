using System.Threading.Tasks;
using Acquirer.Client;
using Acquirer.Client.Domain;
using Microsoft.Extensions.Logging;
using PaymentGateway.Domain.Persistence;

namespace PaymentGateway.Domain.ProcessPayment
{
    public class ProcessPaymentService : IProcessPaymentService
    {
        private readonly IAcquirerClient acquirerClient;
        private readonly ISavePaymentRepository savePaymentRepository;
        private readonly ILogger<ProcessPaymentService> logger;

        public ProcessPaymentService(
            IAcquirerClient acquirerClient, 
            ISavePaymentRepository savePaymentRepository,
            ILogger<ProcessPaymentService> logger)
        {
            this.acquirerClient = acquirerClient;
            this.savePaymentRepository = savePaymentRepository;
            this.logger = logger;
        }

        public async Task<PaymentProcessingResult> Process(CreatePayment createPayment)
        {
            logger.LogInformation("Start processing new payment.");
            var acquirerProcessingResult = await acquirerClient.ProcessPayment(createPayment);
            var savePaymentResult = await savePaymentRepository.Save(
                new ProcessedPayment(
                    createPayment.CardHolder,
                    createPayment.CardNumber,
                    createPayment.ExpiryYear,
                    createPayment.ExpiryMonth,
                    createPayment.Amount,
                    createPayment.Currency,
                    acquirerProcessingResult.AcquirerPaymentId,
                    acquirerProcessingResult.IsPaymentSuccessful));

            logger.LogInformation("Exit processing new payment.");
            return new PaymentProcessingResult(savePaymentResult.Key, acquirerProcessingResult.IsPaymentSuccessful);
        }
    }
}