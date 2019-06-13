using System;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PaymentGateway.Configuration;

namespace PaymentGateway.Infrastructure.Security
{
    public class JwtTokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration configuration;

        public JwtTokenGenerator(IConfiguration configuration)
        {
            this.configuration = configuration;
        }

        public Task<string> GenerateToken()
        {
            var tokenManagement = configuration.GetSection("tokenManagement").Get<TokenManagement>();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                tokenManagement.Issuer,
                tokenManagement.Audience,
                null,
                expires: DateTime.Now.AddMinutes(tokenManagement.AccessExpiration),
                signingCredentials: credentials
            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return Task.FromResult(token);
        }
    }
}