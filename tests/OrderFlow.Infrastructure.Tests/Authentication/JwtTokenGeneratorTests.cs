using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OrderFlow.Infrastructure.Authentication;
using System.Security.Claims;
using System.Text;

namespace OrderFlow.Infrastructure.Tests.Authentication
{
    public class JwtTokenGeneratorTests
    {
        [Fact]
        public async Task GeneratedToken_PassesApiValidationRules()
        {
            const string key = "test-signing-key-at-least-32-bytes-long!!";

            var options = new JwtOptions
            {
                Issuer = "OrderFlow.Api",
                Audience = "OrderFlow.Client",
                SecretKey = key,
                AccessTokenExpirationMinutes = 15
            };

            var generator = new JwtTokenGenerator(Options.Create(options));

            var userId = Guid.NewGuid();

            var token = generator.GenerateToken(userId, "customer@example.com", ["Customer"]);

            var result = await new JsonWebTokenHandler()
                .ValidateTokenAsync(
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        ValidIssuer = options.Issuer,
                        ValidAudience = options.Audience,

                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),

                        NameClaimType = "sub",
                        RoleClaimType = "role"
                    });

            Assert.True(result.IsValid);
            Assert.Equal(userId.ToString(), result.ClaimsIdentity.Name);

            var principal = new ClaimsPrincipal(result.ClaimsIdentity);

            Assert.True(principal.IsInRole("Customer"));
        }
        [Fact]
        public void GenerateToken_WithShortSecretKey_ThrowsException()
        {
            var options = new JwtOptions
            {
                Issuer = "OrderFlow.Api",
                Audience = "OrderFlow.Client",
                SecretKey = "too-short",
                AccessTokenExpirationMinutes = 15
            };

            var generator = new JwtTokenGenerator(Options.Create(options));

            Assert.Throws<InvalidOperationException>(() =>
                generator.GenerateToken(
                    Guid.NewGuid(),
                    "customer@example.com",
                    ["Customer"]));
        }
    }
}
