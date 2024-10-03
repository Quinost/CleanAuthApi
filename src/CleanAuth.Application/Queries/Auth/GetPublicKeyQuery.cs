namespace CleanAuth.Application.Queries.Auth;
public record GetPublicKeyQuery() : IRequest<string>;

internal sealed class GetPublicKeyQueryHandler(IRsaKeyRepository rsaKeyRepository) : IRequestHandler<GetPublicKeyQuery, string>
{
    public Task<string> Handle(GetPublicKeyQuery request, CancellationToken cancellationToken) 
        => Task.FromResult(rsaKeyRepository.GetPublicKey());
}
