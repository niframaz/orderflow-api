namespace OrderFlow.Application.Abstractions.Security
{
    public interface IJwtTokenGenerator
    {
        string GenerateToken(Guid userId, string email, IEnumerable<string> roles);
    }
}
