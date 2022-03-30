
using System.Security.Claims;

using AspNetCore.Authentication.ApiKey;

using IdentityModel;

namespace Messenger.Authentication;

public class ApiKeyProvider : IApiKeyProvider
{
    private readonly ILogger<IApiKeyProvider> _logger;

    public ApiKeyProvider(ILogger<IApiKeyProvider> logger)
    {
        _logger = logger;
    }

    public Task<IApiKey> ProvideAsync(string key)
    {
        try
        {
            if (key != "foobar")
            {
                throw new UnauthorizedAccessException();
            }

            return Task.FromResult<IApiKey>(new ApiKey(key, "api", new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, "api"),
                    new Claim(ClaimTypes.NameIdentifier, "api")
                }));
        }
        catch (System.Exception exception)
        {
            _logger.LogError(exception, exception.Message);
            throw;
        }
    }
}
