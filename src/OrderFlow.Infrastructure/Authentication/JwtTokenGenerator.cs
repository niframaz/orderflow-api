using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using OrderFlow.Application.Abstractions.Security;

namespace OrderFlow.Infrastructure.Authentication
{
    public class JwtTokenGenerator(IOptions<JwtOptions> jwtOptions) : IJwtTokenGenerator
    {
        private readonly JwtOptions _options = jwtOptions.Value;

        public string GenerateToken(Guid userId, string email, IEnumerable<string> roles)
        {
            if (Encoding.UTF8.GetByteCount(_options.SecretKey) < 32)
                throw new InvalidOperationException(
                    "JWT secret key must be at least 32 bytes.");

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, userId.ToString()),
                new(JwtRegisteredClaimNames.Email, email),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var role in roles.Distinct())
            {
                claims.Add(new Claim("role", role));
            }

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));

            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var now = DateTime.UtcNow;

            var descriptor = new SecurityTokenDescriptor
            {
                Issuer = _options.Issuer,
                Audience = _options.Audience,
                Subject = new ClaimsIdentity(claims),
                IssuedAt = now,
                NotBefore = now,
                Expires = now.AddMinutes(_options.AccessTokenExpirationMinutes),
                SigningCredentials = signingCredentials
            };

            return new JsonWebTokenHandler().CreateToken(descriptor);
        }
    }
}
