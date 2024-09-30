using CleanAuth.Domain.Interfaces;
using CleanAuth.Infrastructure.Auth;

namespace CleanAuth.Infrastructure.Repositories;
public class RsaKeyRepository(JwtConfig jwtConfig) : IRsaKeyRepository
{
    public string GetPublicKey() => jwtConfig.PublicKeyPEM;
}
