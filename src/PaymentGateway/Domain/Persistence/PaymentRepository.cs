using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LaYumba.Functional;
using PaymentGateway.Domain.Entities;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Models;
using static LaYumba.Functional.F;

namespace PaymentGateway.Domain.Persistence
{
    public class PaymentRepository: IReadPaymentRepository, ISavePaymentRepository
    {
        private readonly IIdentityGenerator<Guid> identityGenerator;
        private readonly IDictionary<Guid, PaymentEntity> db = new Dictionary<Guid, PaymentEntity>();

        public PaymentRepository(IIdentityGenerator<Guid> identityGenerator)
        {
            this.identityGenerator = identityGenerator;
        }

        public async Task<Option<PaymentDetails>> Read(Guid key)
        {
            if (!db.ContainsKey(key))
                return None;

            var paymentEntity = db[key];
            return await Task.FromResult(Some(new PaymentDetails(
                key, 
                paymentEntity.CardNumber,
                paymentEntity.ExpiryYear,
                paymentEntity.ExpiryMonth,
                paymentEntity.IsPaymentSuccessful)));
        }

        public async Task<SavePaymentResult> Save(ProcessedPayment processedPayment)
        {
            var paymentEntity = new PaymentEntity(
                identityGenerator.NewId,
                processedPayment.CardNumber,
                processedPayment.ExpiryYear,
                processedPayment.ExpiryMonth,
                processedPayment.Amount,
                processedPayment.Currency,
                processedPayment.AcquirerPaymentId,
                processedPayment.IsPaymentSuccessful);
            db.Add(paymentEntity.Key, paymentEntity);
            return await Task.FromResult(new SavePaymentResult(paymentEntity.Key));
        }
    }
}