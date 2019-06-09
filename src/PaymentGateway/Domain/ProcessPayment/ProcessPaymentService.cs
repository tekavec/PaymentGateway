using System.Threading.Tasks;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Models;

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

        public async Task<PaymentProcessingResult> Process(MakePaymentV1 makePayment)
        {
            var acquirerProcessingResult = await acquirerClient.ProcessPayment(makePayment);
            var savePaymentResult = await savePaymentRepository.Save(
                new NewPayment(
                    makePayment.CardHolder,
                    makePayment.CardNumber,
                    makePayment.ExpiryYear,
                    makePayment.ExpiryMonth,
                    makePayment.Amount,
                    makePayment.Currency,
                    acquirerProcessingResult.AcquirerPaymentId,
                    acquirerProcessingResult.Status.ToString(),
                    acquirerProcessingResult.ProcessedAt));
            return new PaymentProcessingResult(savePaymentResult.Key, acquirerProcessingResult.Status);
        }
    }
}