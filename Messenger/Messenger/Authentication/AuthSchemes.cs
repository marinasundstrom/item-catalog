
using AspNetCore.Authentication.ApiKey;

using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Messenger.Authentication;

public static class AuthSchemes
{
    public const string Default =
        JwtBearerDefaults.AuthenticationScheme + "," +
        ApiKeyDefaults.AuthenticationScheme;
}