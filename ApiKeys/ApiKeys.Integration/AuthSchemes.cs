
using AspNetCore.Authentication.ApiKey;

namespace Catalog.ApiKeys;

public static class AuthSchemes
{
    public const string Default =
        //JwtBearerDefaults.AuthenticationScheme + "," +
        ApiKeyDefaults.AuthenticationScheme;
}