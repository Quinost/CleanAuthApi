namespace CleanAuth.Domain.Interfaces;

public interface IRsaKeyRepository
{
    string GetPublicKey();
}