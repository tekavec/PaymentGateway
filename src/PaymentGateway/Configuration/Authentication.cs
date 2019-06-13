using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PaymentGateway.Configuration
{
    public static class Authentication
    { 
        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<TokenManagement>(configuration.GetSection("tokenManagement"));
            var tokenManagement = configuration.GetSection("tokenManagement").Get<TokenManagement>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(x =>
                {
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenManagement.Secret)),
                        ValidIssuer = tokenManagement.Issuer,
                        ValidAudience = tokenManagement.Audience,
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true
                    };
                });
        }

        public static void AddSwaggerSecurityRequirements(this SwaggerGenOptions options)
        {
            options.AddSecurityDefinition("Bearer", new ApiKeyScheme
            {
                In = "header",
                Description = "Please enter into field the word 'Bearer' following by space and JWT",
                Name = "Authorization"
            });
            options.AddSecurityRequirement(
                new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", Enumerable.Empty<string>()}
                }
            );
        }
    }
}