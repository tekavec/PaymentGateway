using System;
using System.IO;
using System.Reflection;
using Acquirer.Client;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PaymentGateway.Domain.Persistence;
using PaymentGateway.Domain.ProcessPayment;
using PaymentGateway.Domain.RetrievePayment;
using PaymentGateway.Infrastructure.Security;
using PaymentGateway.Infrastructure.Swagger;
using PaymentGateway.Models;
using PaymentGateway.Validation;
using Swashbuckle.AspNetCore.Swagger;

namespace PaymentGateway.Configuration
{
    public static class Api
    {
        private static readonly PaymentRepository
            PaymentRepository = new PaymentRepository(new GuidIdentityGenerator());

        public static void RegisterDependencies(this IServiceCollection services)
        {
            services.AddSingleton<IProcessPaymentService, ProcessPaymentService>();
            services.AddSingleton<IRetrievePaymentService, RetrievePaymentService>();
            services.AddSingleton<ISavePaymentRepository>(_ => PaymentRepository);
            services.AddSingleton<IReadPaymentRepository>(_ => PaymentRepository);
            services.AddSingleton<IIdentityGenerator<Guid>, GuidIdentityGenerator>();
            services.AddSingleton<ITokenGenerator, JwtTokenGenerator>();
        }

        public static void RegisterValidators(this IServiceCollection services)
        {
            services.AddTransient<IValidator<MakePaymentV1>>(_ => new MakePaymentV1Validator(() => DateTime.Now));
        }

        public static void RegisterHttpClients(this IServiceCollection services)
        {
            services.AddHttpClient<IAcquirerClient, AcquirerClient>(client =>
                client.BaseAddress = new Uri("http://acquirer:8083"));
        }

        public static void RegisterSwaggerGeneration(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Payment Gateway", Version = "v1" });
                var xmlCommentsFilePath = Path.Combine(AppContext.BaseDirectory,
                    $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");
                if (File.Exists(xmlCommentsFilePath))
                {
                    c.IncludeXmlComments(xmlCommentsFilePath);
                }
                else
                {
                    //TODO Log.Warning($"Could not find XML comments file: {xmlCommentsFilePath}")
                }

                c.CustomSchemaIds(StripOutVersionNumberSuffix);
                c.OperationFilter<CorrelationIdOperationFilter>();
                c.AddSwaggerSecurityRequirements();
            });
        }

        private static string StripOutVersionNumberSuffix(Type type) => type.Name.Substring(0, type.Name.Length - 2);
    }
}