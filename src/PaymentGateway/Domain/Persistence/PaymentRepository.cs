using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaYumba.Functional;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using static LaYumba.Functional.F;

namespace PaymentGateway.Domain.Persistence
{
    public class PaymentRepository: IReadPaymentRepository, ISavePaymentRepository
    {
        private readonly IIdentityGenerator<Guid> identityGenerator;
        private readonly IDictionary<Guid, PaymentEntity> cache = new Dictionary<Guid, PaymentEntity>();

        public PaymentRepository(IIdentityGenerator<Guid> identityGenerator)
        {
            this.identityGenerator = identityGenerator;
        }

        public async Task<Option<PaymentDetails>> Read(Guid key)
        {
            if (!cache.ContainsKey(key))
                return None;

            var paymentEntity = cache[key];
            return Some(new PaymentDetails(
                key, 
                paymentEntity.CardHolder,
                paymentEntity.CardNumber,
                paymentEntity.ExpiryYear,
                paymentEntity.ExpiryMonth,
                paymentEntity.ProcessedStatus,
                paymentEntity.ProcessedAt));
        }

        public async Task<SavePaymentResult> Save(NewPayment newPayment)
        {
            var paymentEntity = new PaymentEntity(
                identityGenerator.NewId,
                newPayment.CardHolder,
                newPayment.CardNumber,
                newPayment.ExpiryYear,
                newPayment.ExpiryMonth,
                newPayment.Amount,
                newPayment.Currency,
                newPayment.AcquirerPaymentId,
                newPayment.ProcessedStatus,
                newPayment.ProcessedAt
                );
            cache.Add(paymentEntity.Key, paymentEntity);
            return new SavePaymentResult(paymentEntity.Key);
        }
    }
}