using System;
using System.Threading.Tasks;
using Acquirer.Client;
using Acquirer.Client.Domain;
using PaymentGateway.Domain.Persistence;

namespace PaymentGateway.Domain.ProcessPayment
{
    public class ProcessPaymentService : IProcessPaymentService
    {
        private readonly IAcquirerClient acquirerClient;
        private readonly ISavePaymentRepository savePaymentRepository;

        public ProcessPaymentService(
            IAcquirerClient acquirerClient, 
            ISavePaymentRepository savePaymentRepository)
        {
            this.acquirerClient = acquirerClient;
            this.savePaymentRepository = savePaymentRepository;
        }

        public async Task<PaymentProcessingResult> Process(CreatePayment createPayment)
        {
            var uri = new Uri("http://localhost");
            var acquirerProcessingResult = await acquirerClient.ProcessPayment(createPayment, uri);
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
            return new PaymentProcessingResult(savePaymentResult.Key, acquirerProcessingResult.IsPaymentSuccessful);
        }
    }
}