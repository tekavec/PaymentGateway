using System;
using System.Collections.Generic;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaymentGateway.Infrastructure.Swagger
{
    public sealed class CorrelationIdOperationFilter : IOperationFilter
    {
        public void Apply(
            Swashbuckle.AspNetCore.Swagger.Operation operation, 
            OperationFilterContext context)
        {
            if (operation.Parameters == null)
            {
                operation.Parameters = new List<IParameter>();
            }

            operation.Parameters.Add(
                new NonBodyParameter
                {
                    Name = "X-Correlation-Id",
                    Type = "string",
                    In = "header",
                    Required = false,
                    Description = "Correlation id for easier request chain tracking",
                    Default = Guid.NewGuid().ToString("D")
                });
        }
    }
}