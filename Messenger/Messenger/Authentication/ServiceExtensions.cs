using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using AspNetCore.Authentication.ApiKey;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.Messenger.Authentication;

public static class ServiceExtensions
{
    public static IServiceCollection AddAuthWithJwt(this IServiceCollection services)
    {
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                    {
                        options.Authority = "https://identity.local";
                        options.Audience = "myapi";

                        options.TokenValidationParameters = new TokenValidationParameters()
                        {
                            NameClaimType = "name"
                        };

                        options.Events = new JwtBearerEvents
                        {
                                OnTokenValidated = context =>
                                {
                                    // Add the access_token as a claim, as we may actually need it
                                    var accessToken = context.SecurityToken as JwtSecurityToken;
                                    if (accessToken != null)
                                    {
                                        ClaimsIdentity? identity = context?.Principal?.Identity as ClaimsIdentity;
                                        if (identity != null)
                                        {
                                            identity.AddClaim(new Claim("access_token", accessToken.RawData));
                                        }
                                    }

                                    return Task.CompletedTask;
                                }
                        };
                        
                        //options.TokenValidationParameters.ValidateAudience = false;

                        //options.Audience = "openid";

                        //options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    });

        return services;
    }

    public static IServiceCollection AddAuthWithApiKey(this IServiceCollection services)
    {
        services.AddAuthentication(ApiKeyDefaults.AuthenticationScheme)

            // The below AddApiKeyInHeaderOrQueryParams without type parameter will require options.Events.OnValidateKey delegete to be set.
            //.AddApiKeyInHeaderOrQueryParams(options =>

            // The below AddApiKeyInHeaderOrQueryParams with type parameter will add the ApiKeyProvider to the dependency container. 
            .AddApiKeyInHeaderOrQueryParams<ApiKeyProvider>(options =>
            {
                options.Realm = "Identity Service API";
                options.KeyName = "X-API-KEY";
            });

        return services;
    }
}

