using System;
using Acquirer.Client;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Models;
using PaymentGateway.Validation;

namespace PaymentGateway.IoC
{
    public static class Api
    {
        private static readonly PaymentRepository PaymentRepository = new PaymentRepository(new GuidIdentityGenerator());

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IProcessPaymentService, ProcessPaymentService>();
            services.AddSingleton<IRetrievePaymentService, RetrievePaymentService>();
            services.AddSingleton<ISavePaymentRepository>(_ => PaymentRepository);
            services.AddSingleton<IReadPaymentRepository>(_ => PaymentRepository);
            services.AddSingleton<IIdentityGenerator<Guid>, GuidIdentityGenerator>();
        }

        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<MakePaymentV1>>(_ => new MakePaymentV1Validator(() => DateTime.Now));
        }

        public static void RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAcquirerClient, AcquirerClient>(client =>
                client.BaseAddress = new Uri("http://localhost:8083"));
        }
    }
}