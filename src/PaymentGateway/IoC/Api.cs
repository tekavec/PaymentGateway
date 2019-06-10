using System;
using Acquirer.Client;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;

namespace PaymentGateway.IoC
{
    public static class Api
    {
        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IProcessPaymentService, ProcessPaymentService>();
            services.AddSingleton<IRetrievePaymentService, RetrievePaymentService>();
            services.AddSingleton<ISavePaymentRepository, PaymentRepository>();
            services.AddSingleton<IReadPaymentRepository, PaymentRepository>();
            services.AddSingleton<IIdentityGenerator<Guid>, GuidIdentityGenerator>();
        }
        public static void RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAcquirerClient, AcquirerClient>(client =>
                client.BaseAddress = new Uri("http://localhost:8083"));
        }
    }
}