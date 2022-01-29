using Duende.IdentityServer.Models;

using IdentityModel;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
          new IdentityResource[]
          {
                new IdentityResources.OpenId(),

                // let's include the role claim in the profile
                new ProfileWithRoleIdentityResource(),
                new IdentityResources.Email()
          };

    public static IEnumerable<ApiResource> ApiResources =>
        new ApiResource[]
        {
                // the api requires the role claim
                new ApiResource("myapi", "The Web Api", new[] { JwtClaimTypes.Role })
                {
                    Scopes = new string[] { "myapi" }
                }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
                    // the api requires the role claim
                    new ApiScope("myapi", "Access the api", new[] { JwtClaimTypes.Role })
        };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "blazor",
                AllowedGrantTypes = GrantTypes.Code,
                RequirePkce = true,
                RequireClientSecret = false,
                AllowedCorsOrigins = { "https://localhost:8080" },
                AllowedScopes = { "openid", "profile", "email", "myapi" },
                RedirectUris = { "https://localhost:8080/authentication/login-callback" },
                PostLogoutRedirectUris = { "https://localhost:8080/" },
                Enabled = true
            }
        };

}
