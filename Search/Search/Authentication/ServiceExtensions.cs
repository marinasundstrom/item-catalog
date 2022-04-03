﻿
using AspNetCore.Authentication.ApiKey;

using Catalog.ApiKeys;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Catalog.Search.Authentication;

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

                        //options.TokenValidationParameters.ValidateAudience = false;

                        //options.Audience = "openid";

                        //options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                    });

        return services;
    }

    public static IServiceCollection AddAuthWithApiKey(this IServiceCollection services)
    {
        services.AddApiKeyAuthentication("https://localhost/apikeys/");

        return services;
    }
}

